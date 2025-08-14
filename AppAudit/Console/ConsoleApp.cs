
using AppAudit.Core;

namespace AppAudit.Console
{
    internal sealed class ConsoleApp
    {
        private readonly AppOptions _opt;

        public ConsoleApp(AppOptions opt)
        {
            _opt = opt;
        }

        public async Task<int> RunAsync()
        {
            using var cts = new CancellationTokenSource();
            System.Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

            System.Console.WriteLine(
                "Start (console)" + Environment.NewLine +
                $"interval: {_opt.IntervalMinutes} min" + Environment.NewLine +
                $"csv: {_opt.CsvPath}");

            // TODO: Skan rejestru i zapis do CSV
            if (_opt.Once) return 0;
            try { await Task.Delay(TimeSpan.FromMinutes(_opt.IntervalMinutes), cts.Token); }
            catch (TaskCanceledException) { }
            return 0;
        }
    }

}
