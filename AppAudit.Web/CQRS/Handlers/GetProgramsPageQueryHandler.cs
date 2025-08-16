using AppAudit.Contracts;
using AppAudit.Contracts.Paging;
using AppAudit.Contracts.Programs.Queries;
using MediatR;

namespace AppAudit.Web.CQRS.Handlers;

public sealed class GetProgramsPageQueryHandler(WebApiClient apiClient)
    : IRequestHandler<GetProgramsPageQuery, PagedResult<ProgramRecord>>
{
    public async Task<PagedResult<ProgramRecord>> Handle(GetProgramsPageQuery request, CancellationToken ct)
    {
        var url = "/api/programs" + request.ToQueryString();

        var res = await apiClient.GetAsync<PagedResult<ProgramRecord>>(url, ct);

        return res ?? new PagedResult<ProgramRecord>([], 0);
    }
}