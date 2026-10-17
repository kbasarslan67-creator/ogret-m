using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using TipEgitim.Application.Auditing;
using TipEgitim.Infrastructure.Configuration;
using TipEgitim.Infrastructure.Persistence;

namespace TipEgitim.Infrastructure.Tests;

[SuppressMessage("Naming", "CA1707", Justification = "Test adları CLAUDE.md standardı gereği S_<MODÜL>_<NO>_<Davranış> biçimindedir.")]
public sealed class SqliteAuditLogTests : IDisposable
{
    private static readonly DateTimeOffset FixedNow = new(2026, 10, 17, 9, 30, 0, TimeSpan.Zero);

    private readonly string _databasePath = Path.Combine(
        Path.GetTempPath(),
        $"tipegitim-audit-{Guid.NewGuid():N}.db");

    private readonly List<SqliteAuditLog> _logs = [];

    [Fact]
    public async Task S_DB_01_AppendThenListReturnsEntry()
    {
        var log = CreateLog();
        var entry = CreateEntry("first", reason: "Dönem kapandı");

        var appended = await log.AppendAsync(entry, CancellationToken.None);
        var listed = await log.ListAsync(CancellationToken.None);

        Assert.True(appended.IsSuccess);
        Assert.Equal(entry, Assert.Single(listed));
    }

    [Fact]
    public async Task S_DB_01_NullReasonRoundTripsAsNull()
    {
        var log = CreateLog();
        var entry = CreateEntry("no-reason", reason: null);

        await log.AppendAsync(entry, CancellationToken.None);
        var listed = await log.ListAsync(CancellationToken.None);

        Assert.Null(Assert.Single(listed).Reason);
    }

    [Fact]
    public async Task S_DB_02_ListKeepsInsertionOrderAcrossThreeAppends()
    {
        var log = CreateLog();

        await log.AppendAsync(CreateEntry("first"), CancellationToken.None);
        await log.AppendAsync(CreateEntry("second"), CancellationToken.None);
        await log.AppendAsync(CreateEntry("third"), CancellationToken.None);

        var listed = await log.ListAsync(CancellationToken.None);

        Assert.Equal(["first", "second", "third"], listed.Select(entry => entry.Action));
    }

    [Fact]
    public async Task S_DB_03_NewInstanceOnSameFileSeesEarlierEntries()
    {
        var writer = CreateLog();
        await writer.AppendAsync(CreateEntry("persisted"), CancellationToken.None);

        var reader = CreateLog();
        var listed = await reader.ListAsync(CancellationToken.None);

        Assert.Equal("persisted", Assert.Single(listed).Action);
    }

    [Fact]
    public async Task S_DB_04_RawUpdateIsRejectedByTrigger()
    {
        var log = CreateLog();
        await log.AppendAsync(CreateEntry("original"), CancellationToken.None);

        await using var connection = new SqliteConnection($"Data Source={_databasePath};Pooling=False");
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "UPDATE audit_log SET action = 'tampered';";

        var exception = await Assert.ThrowsAsync<SqliteException>(() => command.ExecuteNonQueryAsync());

        Assert.Contains("append-only", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task S_DB_05_RawDeleteIsRejectedByTrigger()
    {
        var log = CreateLog();
        await log.AppendAsync(CreateEntry("original"), CancellationToken.None);

        await using var connection = new SqliteConnection($"Data Source={_databasePath};Pooling=False");
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM audit_log;";

        var exception = await Assert.ThrowsAsync<SqliteException>(() => command.ExecuteNonQueryAsync());

        Assert.Contains("append-only", exception.Message, StringComparison.Ordinal);
        Assert.Single(await log.ListAsync(CancellationToken.None));
    }

    [Fact]
    public async Task S_DB_06_EnablesWalJournalMode()
    {
        var log = CreateLog();
        await log.AppendAsync(CreateEntry("first"), CancellationToken.None);

        await using var connection = new SqliteConnection($"Data Source={_databasePath};Pooling=False");
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "PRAGMA journal_mode;";

        var mode = await command.ExecuteScalarAsync();

        Assert.Equal("wal", mode);
    }

    public void Dispose()
    {
        foreach (var log in _logs)
        {
            log.Dispose();
        }

        SqliteConnection.ClearAllPools();
        foreach (var path in new[] { _databasePath, _databasePath + "-wal", _databasePath + "-shm" })
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    private SqliteAuditLog CreateLog()
    {
        var log = new SqliteAuditLog(Options.Create(new PlatformOptions { DatabasePath = _databasePath }));
        _logs.Add(log);
        return log;
    }

    private static AuditEntry CreateEntry(string action, string? reason = "gerekçe") =>
        new(Guid.CreateVersion7(), FixedNow, "kullanici-1", action, "CurriculumYear", "2026-A", reason);
}
