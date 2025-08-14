namespace AppAudit.Infrastructure.Scanners;

using AppAudit.Core.Models;

public interface IProgramScanner
{
    IReadOnlyList<ProgramEntry> ScanAll();
}
