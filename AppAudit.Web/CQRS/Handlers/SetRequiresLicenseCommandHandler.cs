using AppAudit.Contracts.Programs.Commands;
using MediatR;

namespace AppAudit.Web.CQRS.Handlers;

public sealed class SetRequiresLicenseCommandHandler(IHttpClientFactory httpFactory)
    : IRequestHandler<SetRequiresLicenseCommand>
{
    public async Task<Unit> Handle(SetRequiresLicenseCommand request, CancellationToken ct)
    {
        var http = httpFactory.CreateClient("Api");
        var url = $"/api/programs/{request.ProgramId:D}/requires-license?requires={(request.Requires ? "true" : "false")}";
        using var resp = await http.PatchAsync(url, content: null, ct);
        resp.EnsureSuccessStatusCode();
        return Unit.Value;
    }

    Task IRequestHandler<SetRequiresLicenseCommand>.Handle(SetRequiresLicenseCommand request, CancellationToken cancellationToken)
    {
        return Handle(request, cancellationToken);
    }
}