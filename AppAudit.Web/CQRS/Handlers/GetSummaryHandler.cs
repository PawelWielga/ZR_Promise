using AppAudit.Web.CQRS.Queries;
using AppAudit.Web.Models;
using MediatR;

namespace AppAudit.Web.CQRS.Handlers;

internal sealed class GetSummaryHandler(IMediator mediator) : IRequestHandler<GetSummaryQuery, SummaryDto>
{
    public async Task<SummaryDto> Handle(GetSummaryQuery request, CancellationToken ct)
    {
        var all = await mediator.Send(new GetAllProgramsQuery(), ct);
        var total = all.Count;
        var req = all.Count(x => x.RequiresLicense);
        var withoutKey = all.Count(x => x.RequiresLicense && string.IsNullOrWhiteSpace(x.LicenseKey));
        return new SummaryDto(total, req, withoutKey);
    }
}
