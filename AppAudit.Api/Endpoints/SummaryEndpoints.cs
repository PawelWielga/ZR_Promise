using AppAudit.Contracts.Summary.Queries;
using MediatR;

namespace AppAudit.Api.Endpoints;

public static class SummaryEndpoints
{
    public static IEndpointRouteBuilder MapSummaryEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/summary", async (IMediator mediator)
            => Results.Ok(await mediator.Send(new GetSummaryQuery())));

        return app;
    }
}