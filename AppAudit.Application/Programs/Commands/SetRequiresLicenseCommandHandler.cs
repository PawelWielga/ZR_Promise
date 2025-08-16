using AppAudit.Application.Abstractions;
using AppAudit.Contracts.Programs.Commands;
using MediatR;

namespace AppAudit.Application.Programs.Commands;

public sealed class SetRequiresLicenseCommandHandler(IAppDbContext db)
    : IRequestHandler<SetRequiresLicenseCommand>
{
    public async Task<Unit> Handle(SetRequiresLicenseCommand request, CancellationToken ct)
    {
        var p = await db.FindProgramAsync(request.ProgramId, ct);
        if (p is null) return Unit.Value;

        if (!request.Requires && !string.IsNullOrWhiteSpace(p.LicenseKey))
            return Unit.Value;

        p.RequiresLicense = request.Requires;
        await db.SaveChangesAsync(ct);
        return Unit.Value;
    }

    Task IRequestHandler<SetRequiresLicenseCommand>.Handle(SetRequiresLicenseCommand request, CancellationToken cancellationToken)
    {
        return Handle(request, cancellationToken);
    }
}