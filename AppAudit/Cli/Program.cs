using AppAudit.Cli.Audit;
using AppAudit.Cli.Options;
using AppAudit.Infrastructure.Deduplication;
using AppAudit.Infrastructure.Scanners;
using AppAudit.Infrastructure.Writers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Runtime.Versioning;

namespace AppAudit.Cli;

[SupportedOSPlatform("windows")]
internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        AppOptions appOptions;
        try
        {
            appOptions = AppOptions.Parse(args);
            if (appOptions.ShowHelp)
            {
                Usage.Print();
                return 0;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[ERROR] {ex.Message}");
            Usage.Print();
            return 2;
        }

        if (appOptions.RunAsService)
        {
            using var host = Host.CreateDefaultBuilder(args)
                .UseWindowsService(options =>
                {
                    options.ServiceName = "AppAudit Service";
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton(appOptions);
                    services.AddSingleton<IDeduplicator>(sp =>
                    {
                        var opt = sp.GetRequiredService<AppOptions>();
                        var statePath = Path.ChangeExtension(opt.CsvPath, ".state.json");
                        return new FileDeduplicator(statePath);
                    });
                    services.AddSingleton<IProgramScanner, RegistryProgramScanner>();
                    services.AddSingleton<IResultWriter>(sp =>
                    {
                        var opt = sp.GetRequiredService<AppOptions>();
                        return opt.Destination == SaveDestination.Api
                            ? new ApiResultWriter(opt.ApiUrl!, opt.DeviceId)
                            : new CsvResultWriter(opt.CsvPath);
                    });
                    services.AddHostedService<AuditWorker>();
                })
                .Build();

            await host.RunAsync();
            return 0;
        }

        var app = new AuditConsole(appOptions);
        return await app.RunAsync();
    }
}
