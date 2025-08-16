using AppAudit.Contracts;
using AppAudit.Contracts.Summary.Queries;
using MediatR;

namespace AppAudit.Web.CQRS.Handlers;

public sealed class GetSummaryQueryHandler(WebApiClient apiClient)
    : IRequestHandler<GetSummaryQuery, SummaryDto>
{
    public async Task<SummaryDto> Handle(GetSummaryQuery request, CancellationToken ct)
    {
        var dto = await apiClient.GetAsync<SummaryDto>("/api/summary", ct);
        return dto ?? new SummaryDto(0, 0, 0);
    }
}