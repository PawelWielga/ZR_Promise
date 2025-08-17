using AppAudit.Application.Abstractions;
using AppAudit.Contracts.Programs.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppAudit.Application.Programs.Commands;

public sealed class UpsertProgramCommandHandler(IAppDbContext db, IClock clock)
    : IRequestHandler<UpsertProgramCommand>
{
    public async Task Handle(UpsertProgramCommand request, CancellationToken ct)
    {
        var dto = request.Program;
        var exists = await db.Programs.AnyAsync(x => x.ProgramId == dto.ProgramId, ct);
        if (!exists)
        {
            var discoveredAt = dto.DiscoveredAt == default ? clock.Now : dto.DiscoveredAt;

            await db.AddProgramAsync(new ProgramEntry
            {
                ProgramId = dto.ProgramId,
                Name = dto.Name,
                Version = dto.Version,
                Publisher = dto.Publisher,
                DiscoveredAt = discoveredAt,
                RequiresLicense = dto.RequiresLicense,
                LicenseKey = dto.LicenseKey
            }, ct);

            await db.SaveChangesAsync(ct);
        }
    }
}
