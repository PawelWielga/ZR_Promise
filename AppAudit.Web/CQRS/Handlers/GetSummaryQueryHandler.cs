using AppAudit.Contracts;
using AppAudit.Contracts.Programs.Queries;
using MediatR;

namespace AppAudit.Web.CQRS.Handlers;

public sealed class GetSummaryQueryHandler(IHttpClientFactory httpFactory)
    : IRequestHandler<GetSummaryQuery, SummaryDto>
{
    public async Task<SummaryDto> Handle(GetSummaryQuery request, CancellationToken ct)
    {
        var http = httpFactory.CreateClient("Api");
        var dto = await http.GetFromJsonAsync<SummaryDto>("/api/summary", ct);
        return dto ?? new SummaryDto(0, 0, 0);
    }
}