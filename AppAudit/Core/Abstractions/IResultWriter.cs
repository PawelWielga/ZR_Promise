namespace AppAudit.Core.Abstractions;

using AppAudit.Core.Models;

public interface IResultWriter
{
    void WriteAll(IEnumerable<ProgramEntry> items, string path, DateTimeOffset eventUtc, string scanId);
}
