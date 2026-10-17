using System.Globalization;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using TipEgitim.Application.Auditing;
using TipEgitim.Infrastructure.Configuration;
using TipEgitim.SharedKernel;

namespace TipEgitim.Infrastructure.Persistence;

/// <summary>
/// SQLite tabanlı, yalnızca ekleme yapılabilen denetim günlüğü (ADR-005).
/// Tablo ilk kullanımda oluşturulur; <c>audit_log</c> üzerinde UPDATE ve DELETE tetikleyicileri
/// kayıtların ham SQL ile bile değiştirilmesini engeller. Her işlem kendi bağlantısını açıp kapatır.
/// </summary>
public sealed class SqliteAuditLog : IAuditLog, IDisposable
{
    private const string AppendOnlyMessage = "audit_log is append-only";
    private const string Iso8601RoundTrip = "O";

    private const string SchemaSql = """
        CREATE TABLE IF NOT EXISTS audit_log (
            id TEXT NOT NULL PRIMARY KEY,
            occurred_at_utc TEXT NOT NULL,
            actor TEXT NOT NULL,
            action TEXT NOT NULL,
            target_type TEXT NOT NULL,
            target_id TEXT NOT NULL,
            reason TEXT NULL
        );
        CREATE TRIGGER IF NOT EXISTS audit_log_block_update BEFORE UPDATE ON audit_log
        BEGIN
            SELECT RAISE(ABORT, 'audit_log is append-only');
        END;
        CREATE TRIGGER IF NOT EXISTS audit_log_block_delete BEFORE DELETE ON audit_log
        BEGIN
            SELECT RAISE(ABORT, 'audit_log is append-only');
        END;
        """;

    private const string InsertSql = """
        INSERT INTO audit_log (id, occurred_at_utc, actor, action, target_type, target_id, reason)
        VALUES ($id, $occurred_at_utc, $actor, $action, $target_type, $target_id, $reason);
        """;

    private const string SelectSql = """
        SELECT id, occurred_at_utc, actor, action, target_type, target_id, reason
        FROM audit_log
        ORDER BY rowid;
        """;

    private readonly string _databasePath;
    private readonly string _connectionString;
    private readonly SemaphoreSlim _schemaGate = new(1, 1);
    private volatile bool _schemaReady;

    /// <param name="options">Platform yapılandırması; <see cref="PlatformOptions.DatabasePath"/> kullanılır.</param>
    public SqliteAuditLog(IOptions<PlatformOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _databasePath = Path.GetFullPath(options.Value.DatabasePath, AppContext.BaseDirectory);
        _connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = _databasePath,
            Mode = SqliteOpenMode.ReadWriteCreate,
            // Havuz kapalı: bağlantı dispose edildiğinde dosya tutamacı hemen serbest kalır.
            Pooling = false,
        }.ToString();
    }

    /// <inheritdoc />
    public async Task<Result> AppendAsync(AuditEntry entry, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entry);
        cancellationToken.ThrowIfCancellationRequested();

        await EnsureSchemaAsync(cancellationToken);

        try
        {
            await using var connection = await OpenConnectionAsync(cancellationToken);
            await using var command = connection.CreateCommand();
            command.CommandText = InsertSql;
            command.Parameters.Add(new SqliteParameter("$id", entry.Id.ToString("D", CultureInfo.InvariantCulture)));
            command.Parameters.Add(new SqliteParameter(
                "$occurred_at_utc",
                entry.OccurredAtUtc.ToUniversalTime().ToString(Iso8601RoundTrip, CultureInfo.InvariantCulture)));
            command.Parameters.Add(new SqliteParameter("$actor", entry.Actor));
            command.Parameters.Add(new SqliteParameter("$action", entry.Action));
            command.Parameters.Add(new SqliteParameter("$target_type", entry.TargetType));
            command.Parameters.Add(new SqliteParameter("$target_id", entry.TargetId));
            command.Parameters.Add(new SqliteParameter("$reason", (object?)entry.Reason ?? DBNull.Value));

            await command.ExecuteNonQueryAsync(cancellationToken);
            return Result.Success();
        }
        catch (SqliteException)
        {
            return Result.Failure(AuditErrors.StorageFailed);
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<AuditEntry>> ListAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await EnsureSchemaAsync(cancellationToken);

        var entries = new List<AuditEntry>();
        await using var connection = await OpenConnectionAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = SelectSql;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            entries.Add(new AuditEntry(
                Guid.Parse(reader.GetString(0), CultureInfo.InvariantCulture),
                DateTimeOffset.ParseExact(reader.GetString(1), Iso8601RoundTrip, CultureInfo.InvariantCulture),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4),
                reader.GetString(5),
                reader.IsDBNull(6) ? null : reader.GetString(6)));
        }

        return entries;
    }

    /// <inheritdoc />
    public void Dispose() => _schemaGate.Dispose();

    private async Task<SqliteConnection> OpenConnectionAsync(CancellationToken cancellationToken)
    {
        var connection = new SqliteConnection(_connectionString);
        try
        {
            await connection.OpenAsync(cancellationToken);

            // Eşzamanlı yazıcılar kilitli dosyada hemen hata almasın diye kısa bir bekleme süresi.
            await using var command = connection.CreateCommand();
            command.CommandText = "PRAGMA busy_timeout = 5000;";
            await command.ExecuteNonQueryAsync(cancellationToken);

            return connection;
        }
        catch
        {
            await connection.DisposeAsync();
            throw;
        }
    }

    private async Task EnsureSchemaAsync(CancellationToken cancellationToken)
    {
        if (_schemaReady)
        {
            return;
        }

        await _schemaGate.WaitAsync(cancellationToken);
        try
        {
            if (_schemaReady)
            {
                return;
            }

            var directory = Path.GetDirectoryName(_databasePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await using var connection = await OpenConnectionAsync(cancellationToken);

            await using (var journal = connection.CreateCommand())
            {
                journal.CommandText = "PRAGMA journal_mode = WAL;";
                await journal.ExecuteNonQueryAsync(cancellationToken);
            }

            await using (var schema = connection.CreateCommand())
            {
                schema.CommandText = SchemaSql;
                await schema.ExecuteNonQueryAsync(cancellationToken);
            }

            _schemaReady = true;
        }
        finally
        {
            _schemaGate.Release();
        }
    }
}
