using AppAudit.Web.Abstractions;
using AppAudit.Web.CQRS.Commands;
using AppAudit.Web.CQRS.Queries;
using AppAudit.Web.Models;
using MediatR;

namespace AppAudit.Web.CQRS.Handlers;

internal sealed class SetLicenseKeyHandler(IProgramsStateStore store, IMediator mediator) : IRequestHandler<SetLicenseKeyCommand>
{
    public async Task Handle(SetLicenseKeyCommand request, CancellationToken ct)
    {
        var all = await mediator.Send(new GetAllProgramsQuery(), ct);
        var dict = all.ToDictionary(x => x.ProgramId, x => new ProgramState(x.ProgramId, x.RequiresLicense, x.LicenseKey));
        if (dict.TryGetValue(request.ProgramId, out var s))
            dict[request.ProgramId] = s with { LicenseKey = request.LicenseKey };
        else
            dict[request.ProgramId] = new ProgramState(request.ProgramId, false, request.LicenseKey);
        await store.SaveAsync(dict.Values, ct);
    }
}