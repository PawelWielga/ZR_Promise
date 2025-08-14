using AppAudit.Core.Models;

namespace AppAudit.Infrastructure.Writers;

internal sealed class CompositeResultWriter(IEnumerable<IResultWriter> writers) : IResultWriter
{
    private readonly IReadOnlyList<IResultWriter> _writers = [.. writers];

    public void WriteAll(IEnumerable<ProgramEntry> items, DateTimeOffset eventUtc, string scanId)
    {
        var snapshot = items.ToList();
        foreach (var w in _writers)
            w.WriteAll(snapshot, eventUtc, scanId);
    }
}
