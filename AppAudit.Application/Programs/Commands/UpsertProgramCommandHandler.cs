using AppAudit.Application.Abstractions;
using AppAudit.Contracts.Programs.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppAudit.Application.Programs.Commands;

public sealed class UpsertProgramCommandHandler(IAppDbContext db)
    : IRequestHandler<UpsertProgramCommand>
{
    public async Task<Unit> Handle(UpsertProgramCommand request, CancellationToken ct)
    {
        var dto = request.Program;
        var exists = await db.Programs.AnyAsync(x => x.ProgramId == dto.ProgramId, ct);
        if (!exists)
        {
            await db.AddProgramAsync(new ProgramEntry
            {
                ProgramId = dto.ProgramId,
                Name = dto.Name,
                Version = dto.Version,
                Publisher = dto.Publisher,
                DiscoveredAt = dto.DiscoveredAt,
                RequiresLicense = dto.RequiresLicense,
                LicenseKey = dto.LicenseKey
            }, ct);
            await db.SaveChangesAsync(ct);
        }
        return Unit.Value;
    }

    Task IRequestHandler<UpsertProgramCommand>.Handle(UpsertProgramCommand request, CancellationToken cancellationToken)
    {
        return Handle(request, cancellationToken);
    }
}
