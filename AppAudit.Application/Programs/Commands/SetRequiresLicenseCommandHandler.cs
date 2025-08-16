using AppAudit.Application.Abstractions;
using AppAudit.Contracts.Programs.Commands;
using MediatR;

namespace AppAudit.Application.Programs.Commands;

public sealed class SetRequiresLicenseCommandHandler(IAppDbContext db)
    : IRequestHandler<SetRequiresLicenseCommand>
{
    public async Task Handle(SetRequiresLicenseCommand request, CancellationToken ct)
    {
        var p = await db.FindProgramAsync(request.ProgramId, ct);
        if (p is null) return;

        if (!request.Requires && !string.IsNullOrWhiteSpace(p.LicenseKey))
            return;

        p.RequiresLicense = request.Requires;
        await db.SaveChangesAsync(ct);
    }
}