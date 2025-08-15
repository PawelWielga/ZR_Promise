using AppAudit.Application.Abstractions;
using AppAudit.Contracts.Programs.Commands;
using MediatR;

namespace AppAudit.Application.Programs.Commands;

public sealed class SetLicenseKeyCommandHandler(IAppDbContext db)
    : IRequestHandler<SetLicenseKeyCommand>
{
    public async Task<Unit> Handle(SetLicenseKeyCommand request, CancellationToken ct)
    {
        var p = await db.FindProgramAsync(request.ProgramId, ct);
        if (p is null) return Unit.Value;
        p.LicenseKey = string.IsNullOrWhiteSpace(request.LicenseKey) ? null : request.LicenseKey.Trim();
        await db.SaveChangesAsync(ct);
        return Unit.Value;
    }

    Task IRequestHandler<SetLicenseKeyCommand>.Handle(SetLicenseKeyCommand request, CancellationToken cancellationToken)
    {
        return Handle(request, cancellationToken);
    }
}