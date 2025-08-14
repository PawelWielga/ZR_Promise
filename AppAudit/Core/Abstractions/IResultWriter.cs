using AppAudit.Core.Models;

namespace AppAudit.Core.Abstractions;

public interface IResultWriter
{
    void WriteAll(IEnumerable<ProgramEntry> items, string path);
}
