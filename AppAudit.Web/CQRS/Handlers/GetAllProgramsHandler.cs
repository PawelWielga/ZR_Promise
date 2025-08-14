using AppAudit.Web.Abstractions;
using AppAudit.Web.CQRS.Queries;
using AppAudit.Web.Models;
using MediatR;

namespace AppAudit.Web.CQRS.Handlers;

internal sealed class GetAllProgramsHandler(IProgramsSource source, IProgramsStateStore store) : IRequestHandler<GetAllProgramsQuery, IReadOnlyList<ProgramDto>>
{
    public async Task<IReadOnlyList<ProgramDto>> Handle(GetAllProgramsQuery request, CancellationToken ct)
    {
        var all = await source.GetAllAsync(ct);
        var state = await store.LoadAsync(ct);
        var list = all
            .GroupBy(x => x.ProgramId)
            .Select(g => g.Last())
            .Select(e =>
            {
                state.TryGetValue(e.ProgramId, out var st);
                return ProgramDto.From(e, st?.RequiresLicense ?? false, st?.LicenseKey);
            })
            .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
        return list;
    }
}
