using AppAudit.Application.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(IAppDbContext).Assembly);
});

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
