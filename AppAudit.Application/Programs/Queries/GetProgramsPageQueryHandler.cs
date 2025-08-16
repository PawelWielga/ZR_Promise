using AppAudit.Application.Abstractions;
using AppAudit.Contracts;
using AppAudit.Contracts.Paging;
using AppAudit.Contracts.Programs.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppAudit.Application.Programs.Queries;

public sealed class GetProgramsPageQueryHandler(IAppDbContext db)
    : IRequestHandler<GetProgramsPageQuery, PagedResult<ProgramRecord>>
{
    public async Task<PagedResult<ProgramRecord>> Handle(GetProgramsPageQuery r, CancellationToken ct)
    {
        var query = db.Programs.AsQueryable();

        if (!string.IsNullOrWhiteSpace(r.Search))
        {
            var s = r.Search.Trim();
            query = query.Where(p => p.Name.Contains(s) || (p.Publisher != null && p.Publisher.Contains(s)));
        }

        if (r.RequiresKey is true)
            query = query.Where(p => p.RequiresLicense);

        if (r.HasKey is true)
            query = query.Where(p => !string.IsNullOrWhiteSpace(p.LicenseKey));
        else if (r.HasKey is false)
            query = query.Where(p => string.IsNullOrWhiteSpace(p.LicenseKey));

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(p => p.DiscoveredAt)
            .Skip((Math.Max(1, r.Page) - 1) * Math.Max(1, r.PageSize))
            .Take(Math.Max(1, r.PageSize))
            .Select(p => new ProgramRecord(p.ProgramId, p.Name, p.Version, p.Publisher, p.DiscoveredAt, p.RequiresLicense, p.LicenseKey))
            .ToListAsync(ct);

        return new PagedResult<ProgramRecord>(items, total);
    }
}