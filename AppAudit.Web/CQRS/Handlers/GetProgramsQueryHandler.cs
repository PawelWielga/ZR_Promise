using AppAudit.Contracts;
using AppAudit.Contracts.Programs.Queries;
using MediatR;

namespace AppAudit.Web.CQRS.Handlers;

public sealed class GetProgramsQueryHandler(IHttpClientFactory httpFactory)
    : IRequestHandler<GetProgramsQuery, IReadOnlyList<ProgramRecord>>
{
    public async Task<IReadOnlyList<ProgramRecord>> Handle(GetProgramsQuery request, CancellationToken ct)
    {
        var http = httpFactory.CreateClient("Api");
        var data = await http.GetFromJsonAsync<IReadOnlyList<ProgramRecord>>("/api/programs", ct);
        return data ?? [];
    }
}
