namespace AppAudit.Infrastructure.Writers;

using AppAudit.Core.Models;

public interface IResultWriter
{
    void WriteAll(IEnumerable<ProgramEntry> items, DateTimeOffset eventUtc, string scanId);
}
