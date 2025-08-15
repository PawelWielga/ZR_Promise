using AppAudit.Contracts.Programs.Commands;
using MediatR;

namespace AppAudit.Web.CQRS.Handlers;

public sealed class UpsertProgramCommandHandler(IHttpClientFactory httpFactory)
    : IRequestHandler<UpsertProgramCommand>
{
    public async Task<Unit> Handle(UpsertProgramCommand request, CancellationToken ct)
    {
        var http = httpFactory.CreateClient("Api");
        using var resp = await http.PostAsJsonAsync("/api/programs", request.Program, ct);
        resp.EnsureSuccessStatusCode();
        return Unit.Value;
    }

    Task IRequestHandler<UpsertProgramCommand>.Handle(UpsertProgramCommand request, CancellationToken cancellationToken)
    {
        return Handle(request, cancellationToken);
    }
}