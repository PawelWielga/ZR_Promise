var builder = WebApplication.CreateBuilder(args);

// MediatR – rejestruj handlery z aktualnego assembly (Web)
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

// HttpClient do API (z konfiguracji)
var apiBase = builder.Configuration["Api:BaseUrl"] ?? "http://localhost:5000";
builder.Services.AddHttpClient("Api", client => client.BaseAddress = new Uri(apiBase));

// Blazor
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
