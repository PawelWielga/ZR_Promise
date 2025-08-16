using AppAudit.Contracts.Programs.Commands;
using MediatR;

namespace AppAudit.Web.CQRS.Handlers;

public sealed class UpsertProgramCommandHandler(WebApiClient apiClient)
    : IRequestHandler<UpsertProgramCommand>
{
    public async Task Handle(UpsertProgramCommand request, CancellationToken ct)
    {
        await apiClient.PostAsync("/api/programs", request.Program, ct);
    }
}