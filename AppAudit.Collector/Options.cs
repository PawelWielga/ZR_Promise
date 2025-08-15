namespace AppAudit.Collector;

public sealed class Options
{
    public required Uri ApiBase { get; init; }
    public int IntervalMinutes { get; init; } = 15;

    public static Options Parse(string[] args)
    {
        Uri api = new("http://localhost:5000");
        int minutes = 15;

        for (int i = 0; i < args.Length - 1; i++)
        {
            var k = args[i].ToLowerInvariant();
            var v = args[i + 1];
            if (k is "--api" or "-a") api = new Uri(v);
            if (k is "--minutes" or "-m") minutes = int.Parse(v);
        }

        return new Options { ApiBase = api, IntervalMinutes = Math.Clamp(minutes, 1, 1440) };
    }
}
