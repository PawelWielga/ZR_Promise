using AppAudit.Contracts.Programs.Commands;
using MediatR;

namespace AppAudit.Web.CQRS.Handlers;

public sealed class SetRequiresLicenseCommandHandler(WebApiClient apiClient)
    : IRequestHandler<SetRequiresLicenseCommand>
{
    public async Task Handle(SetRequiresLicenseCommand request, CancellationToken ct)
    {
        var url = $"/api/programs/{request.ProgramId:D}/requires-license?requires={(request.Requires ? "true" : "false")}";
        await apiClient.PatchAsync(url, ct);
    }
}