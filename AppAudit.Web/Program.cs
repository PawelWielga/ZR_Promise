using AppAudit.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

var apiBase = builder.Configuration["Api:BaseUrl"] ?? "http://localhost:5000";
builder.Services.AddHttpClient("Api", client => client.BaseAddress = new Uri(apiBase));
builder.Services.AddScoped<WebApiClient>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
