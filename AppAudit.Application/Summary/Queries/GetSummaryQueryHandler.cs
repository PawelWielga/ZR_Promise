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
        var total = await db.Programs.CountAsync(ct);
        var requiring = await db.Programs.CountAsync(p => p.RequiresLicense, ct);
        var withoutKey = await db.Programs.CountAsync(p => p.RequiresLicense && (p.LicenseKey == null || p.LicenseKey == ""), ct);
        return new SummaryDto(total, requiring, withoutKey);
    }
}
