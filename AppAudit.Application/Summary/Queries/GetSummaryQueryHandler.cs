using AppAudit.Application.Abstractions;
using AppAudit.Contracts;
using AppAudit.Contracts.Summary.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppAudit.Application.Summary.Queries;

public sealed class GetSummaryQueryHandler(IAppDbContext db)
    : IRequestHandler<GetSummaryQuery, SummaryDto>
{
    public async Task<SummaryDto> Handle(GetSummaryQuery request, CancellationToken ct)
    {
        var result = await db.Programs
            .GroupBy(_ => 1)
            .Select(g => new
            {
                Total = g.Count(),
                Requiring = g.Count(p => p.RequiresLicense),
                WithoutKey = g.Count(p => p.RequiresLicense && (p.LicenseKey == null || p.LicenseKey == ""))
            })
            .SingleOrDefaultAsync(ct);

        return result is null
            ? new SummaryDto(0, 0, 0)
            : new SummaryDto(result.Total, result.Requiring, result.WithoutKey);
    }
}
