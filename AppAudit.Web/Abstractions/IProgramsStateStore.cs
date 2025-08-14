using AppAudit.Web.Models;

namespace AppAudit.Web.Abstractions;

internal interface IProgramsStateStore
{
    Task<IReadOnlyDictionary<string, ProgramState>> LoadAsync(CancellationToken ct);
    Task SaveAsync(IEnumerable<ProgramState> states, CancellationToken ct);
}
