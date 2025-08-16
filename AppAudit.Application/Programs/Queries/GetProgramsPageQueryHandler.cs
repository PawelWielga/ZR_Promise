using AppAudit.Application.Abstractions;
using AppAudit.Application.Mapping;
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
        var query = db.Programs.AsQueryable().ApplyFilters(r);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(p => p.DiscoveredAt)
            .Skip((Math.Max(1, r.Page) - 1) * Math.Max(1, r.PageSize))
            .Take(Math.Max(1, r.PageSize))
            .Select(ProgramMappings.ToRecord)
            .ToListAsync(ct);

        return new PagedResult<ProgramRecord>(items, total);
    }
}
