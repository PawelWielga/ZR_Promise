using System.Runtime.Versioning;

namespace AppAudit.Cli;

[SupportedOSPlatform("windows")]
internal static class AuditLoop
{
    public static async Task RunAsync(Action runOnce, bool once, TimeSpan interval, CancellationToken token)
    {
        do
        {
            try { runOnce(); }
            catch (Exception ex) { Console.Error.WriteLine($"[ERROR] {ex.Message}"); }

            if (once) break;
            try { await Task.Delay(interval, token); }
            catch (TaskCanceledException) { break; }
        }
        while (!token.IsCancellationRequested);
    }
}