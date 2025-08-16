using AppAudit.Contracts;
using AppAudit.Contracts.Programs.Commands;
using AppAudit.Contracts.Programs.Queries;
using MediatR;

namespace AppAudit.Api.Endpoints;

public static class ProgramsEndpoints
{
    public static IEndpointRouteBuilder MapProgramsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/programs");

        // GET: lista programów
        group.MapGet("", async (IMediator mediator, [AsParameters] GetProgramsPageRequest request) =>
        {
            var res = await mediator.Send(new GetProgramsPageQuery(
                request.Page,
                request.PageSize,
                request.Search,
                request.RequiresKey,
                request.HasKey
            ));

            return Results.Ok(res);
        });

        // POST: upsert
        group.MapPost("", async (IMediator mediator, ProgramRecord dto) =>
        {
            await mediator.Send(new UpsertProgramCommand(dto));
            return Results.Accepted();
        });

        // PATCH: toggle wymaga licencji
        group.MapPatch("/{id:guid}/requires-license", async (IMediator mediator, Guid id, bool requires) =>
        {
            await mediator.Send(new SetRequiresLicenseCommand(id, requires));
            return Results.NoContent();
        });

        // PUT: ustawienie/wyczyszczenie klucza licencji
        group.MapPut("/{id:guid}/license-key", async (IMediator mediator, Guid id, [AsParameters] SetLicenseKeyRequest request) =>
        {
            await mediator.Send(new SetLicenseKeyCommand(id, request.Key));
            return Results.NoContent();
        });

        return app;
    }
}