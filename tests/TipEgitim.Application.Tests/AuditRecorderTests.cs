using System.Diagnostics.CodeAnalysis;
using TipEgitim.Application.Auditing;
using TipEgitim.SharedKernel;

namespace TipEgitim.Application.Tests;

[SuppressMessage("Naming", "CA1707", Justification = "Test adları CLAUDE.md standardı gereği S_<MODÜL>_<NO>_<Davranış> biçimindedir.")]
public sealed class AuditRecorderTests
{
    private static readonly string[] ExpectedOrder = ["first", "second", "third"];
    private static readonly DateTimeOffset FixedNow = new(2026, 10, 17, 9, 30, 0, TimeSpan.Zero);

    [Fact]
    public async Task S_AUD_01_RecordsEntryWithInjectedTimeAndActor()
    {
        var log = new FakeAuditLog();
        var recorder = new AuditRecorder(log, new FakeTimeProvider(FixedNow));

        var result = await recorder.RecordAsync(
            "kullanici-1", "curriculum.year_frozen", "CurriculumYear", "2026-A", "Dönem kapandı", CancellationToken.None);

        Assert.True(result.IsSuccess);
        var entry = Assert.Single(log.Entries);
        Assert.Equal(FixedNow, entry.OccurredAtUtc);
        Assert.Equal("kullanici-1", entry.Actor);
        Assert.Equal("curriculum.year_frozen", entry.Action);
        Assert.Equal("CurriculumYear", entry.TargetType);
        Assert.Equal("2026-A", entry.TargetId);
        Assert.Equal("Dönem kapandı", entry.Reason);
        Assert.NotEqual(Guid.Empty, entry.Id);
    }

    [Fact]
    public async Task S_AUD_01_RejectsBlankActorWithExpectedCode()
    {
        var log = new FakeAuditLog();
        var recorder = new AuditRecorder(log, new FakeTimeProvider(FixedNow));

        var result = await recorder.RecordAsync(
            "   ", "curriculum.year_frozen", "CurriculumYear", "2026-A", null, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal("audit.actor_required", result.Error?.Code);
        Assert.Equal(ErrorType.Validation, result.Error?.Type);
        Assert.Empty(log.Entries);
    }

    [Fact]
    public void S_AUD_01_IAuditLogExposesNoDeleteOrUpdateMethod()
    {
        var methodNames = typeof(IAuditLog)
            .GetMethods()
            .Select(method => method.Name)
            .Order(StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(["AppendAsync", "ListAsync"], methodNames);
        Assert.DoesNotContain(methodNames, name =>
            name.StartsWith("Delete", StringComparison.Ordinal)
            || name.StartsWith("Remove", StringComparison.Ordinal)
            || name.StartsWith("Update", StringComparison.Ordinal)
            || name.StartsWith("Clear", StringComparison.Ordinal)
            || name.StartsWith("Set", StringComparison.Ordinal));
    }

    [Fact]
    public async Task S_AUD_01_ListsEntriesInAppendOrder()
    {
        var log = new FakeAuditLog();
        var recorder = new AuditRecorder(log, new FakeTimeProvider(FixedNow));

        await recorder.RecordAsync("a", "first", "T", "1", null, CancellationToken.None);
        await recorder.RecordAsync("b", "second", "T", "2", null, CancellationToken.None);
        await recorder.RecordAsync("c", "third", "T", "3", null, CancellationToken.None);

        var listed = await log.ListAsync(CancellationToken.None);

        Assert.Equal(ExpectedOrder, listed.Select(entry => entry.Action));
    }

    private sealed class FakeTimeProvider(DateTimeOffset now) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => now;
    }

    private sealed class FakeAuditLog : IAuditLog
    {
        private readonly List<AuditEntry> _entries = [];

        public IReadOnlyList<AuditEntry> Entries => _entries;

        public Task<Result> AppendAsync(AuditEntry entry, CancellationToken cancellationToken)
        {
            _entries.Add(entry);
            return Task.FromResult(Result.Success());
        }

        public Task<IReadOnlyList<AuditEntry>> ListAsync(CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyList<AuditEntry>>(_entries.ToArray());
    }
}
