using AppAudit.Contracts.Programs.Commands;
using MediatR;

namespace AppAudit.Web.CQRS.Handlers;

public sealed class SetLicenseKeyCommandHandler(IHttpClientFactory httpFactory)
    : IRequestHandler<SetLicenseKeyCommand>
{
    public async Task<Unit> Handle(SetLicenseKeyCommand request, CancellationToken ct)
    {
        var http = httpFactory.CreateClient("Api");
        var key = request.LicenseKey ?? string.Empty;
        var url = $"/api/programs/{request.ProgramId:D}/license-key?key={Uri.EscapeDataString(key)}";
        using var resp = await http.PutAsync(url, content: null, ct);
        resp.EnsureSuccessStatusCode();
        return Unit.Value;
    }

    Task IRequestHandler<SetLicenseKeyCommand>.Handle(SetLicenseKeyCommand request, CancellationToken cancellationToken)
    {
        return Handle(request, cancellationToken);
    }
}