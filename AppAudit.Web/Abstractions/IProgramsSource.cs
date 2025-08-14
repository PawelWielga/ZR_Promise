using AppAudit.Core.Models;

namespace AppAudit.Web.Abstractions;

internal interface IProgramsSource
{
    Task<IReadOnlyList<ProgramEntry>> GetAllAsync(CancellationToken ct);
}
