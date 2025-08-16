using AppAudit.Application.Abstractions;
using AppAudit.Contracts.Programs.Commands;
using MediatR;

namespace AppAudit.Application.Programs.Commands;

public sealed class SetLicenseKeyCommandHandler(IAppDbContext db)
    : IRequestHandler<SetLicenseKeyCommand>
{
    public async Task Handle(SetLicenseKeyCommand request, CancellationToken ct)
    {
        var p = await db.FindProgramAsync(request.ProgramId, ct);
        if (p is null) return;

        var key = string.IsNullOrWhiteSpace(request.LicenseKey) ? null : request.LicenseKey.Trim();
        p.LicenseKey = key;

        if (!string.IsNullOrWhiteSpace(key))
            p.RequiresLicense = true;

        await db.SaveChangesAsync(ct);
    }
}