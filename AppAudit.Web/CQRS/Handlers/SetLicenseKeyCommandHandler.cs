using AppAudit.Contracts.Programs.Commands;
using MediatR;

namespace AppAudit.Web.CQRS.Handlers;

public sealed class SetLicenseKeyCommandHandler(WebApiClient apiClient)
    : IRequestHandler<SetLicenseKeyCommand>
{
    public async Task Handle(SetLicenseKeyCommand request, CancellationToken ct)
    {
        var key = request.LicenseKey ?? string.Empty;
        var url = $"/api/programs/{request.ProgramId:D}/license-key?key={Uri.EscapeDataString(key)}";
        await apiClient.PutAsync(url, ct);
    }
}