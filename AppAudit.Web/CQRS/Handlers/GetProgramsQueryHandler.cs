using AppAudit.Contracts;
using AppAudit.Contracts.Programs.Queries;
using MediatR;

namespace AppAudit.Web.CQRS.Handlers;

public sealed class GetProgramsQueryHandler(WebApiClient apiClient)
    : IRequestHandler<GetProgramsQuery, IReadOnlyList<ProgramRecord>>
{
    public async Task<IReadOnlyList<ProgramRecord>> Handle(GetProgramsQuery request, CancellationToken ct)
    {
        var data = await apiClient.GetAsync<IReadOnlyList<ProgramRecord>>("/api/programs", ct);
        return data ?? [];
    }
}
