using AppAudit.Core;
using System.Runtime.Versioning;

namespace AppAudit.Console;

[SupportedOSPlatform("windows")]
internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        AppOptions opt;
        try
        {
            opt = AppOptions.Parse(args);
            if (opt.ShowHelp)
            {
                Usage.Print();
                return 0;
            }
        }
        catch (Exception ex)
        {
            System.Console.Error.WriteLine($"[ERROR] {ex.Message}");
            Usage.Print();
            return 2;
        }

        if (opt.RunAsService)
        {
            // TODO: Uruchamianie jako usługa
            System.Console.WriteLine("[INFO] Service mode enabled (placeholder)");
            return 0;
        }

        var app = new ConsoleApp(opt);
        return await app.RunAsync();
    }
}

