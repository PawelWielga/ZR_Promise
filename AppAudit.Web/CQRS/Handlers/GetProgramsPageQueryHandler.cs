using AppAudit.Contracts;
using AppAudit.Contracts.Paging;
using AppAudit.Contracts.Programs.Queries;
using MediatR;

namespace AppAudit.Web.CQRS.Handlers;

public sealed class GetProgramsPageQueryHandler(IHttpClientFactory httpFactory)
    : IRequestHandler<GetProgramsPageQuery, PagedResult<ProgramRecord>>
{
    public async Task<PagedResult<ProgramRecord>> Handle(GetProgramsPageQuery request, CancellationToken ct)
    {
        var http = httpFactory.CreateClient("Api");

        var url = $"/api/programs?page={request.Page}&pageSize={request.PageSize}"
                + (string.IsNullOrWhiteSpace(request.Search) ? "" : $"&search={Uri.EscapeDataString(request.Search)}")
                + (request.RequiresKey is null ? "" : $"&requiresKey={(request.RequiresKey.Value ? "true" : "false")}")
                + (request.HasKey is null ? "" : $"&hasKey={(request.HasKey.Value ? "true" : "false")}");

        var res = await http.GetFromJsonAsync<PagedResult<ProgramRecord>>(url, ct);

        return res ?? new PagedResult<ProgramRecord>([], 0);
    }
}