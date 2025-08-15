using AppAudit.Application.Abstractions;
using AppAudit.Application.Mapping;
using AppAudit.Contracts;
using AppAudit.Contracts.Programs.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppAudit.Application.Programs.Queries;

public sealed class GetProgramsQueryHandler(IAppDbContext db)
    : IRequestHandler<GetProgramsQuery, IReadOnlyList<ProgramRecord>>
{
    public async Task<IReadOnlyList<ProgramRecord>> Handle(GetProgramsQuery request, CancellationToken ct)
        => await db.Programs
            .OrderByDescending(p => p.DiscoveredAt)
            .Select(p => p.ToDto())
            .ToListAsync(ct);
}
