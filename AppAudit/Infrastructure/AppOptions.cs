namespace AppAudit.Infrastructure;

internal sealed class AppOptions
{
    public int IntervalMinutes { get; set; } = 1;
    public bool RunAsService { get; set; } = false;
    public bool Once { get; set; } = false;
    public bool ShowHelp { get; set; } = false;
    public string CsvPath { get; set; } = Path.GetFullPath("C:\\tmp\\programs_log.csv");

    public static AppOptions Parse(string[] args)
    {
        var o = new AppOptions();
        for (int i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            switch (arg)
            {
                case "--help":
                case "-h":
                case "/?":
                    o.ShowHelp = true; break;

                case "--interval-minutes":
                    RequireValue(args, ++i, arg, out var mStr);
                    if (!int.TryParse(mStr, out var m) || m <= 0)
                        throw new ArgumentException("Invalid value for --interval-minutes.");
                    o.IntervalMinutes = m; break;

                case "--service":
                    o.RunAsService = true; break;

                case "--once":
                    o.Once = true; break;

                case "--csv-path":
                    RequireValue(args, ++i, arg, out var csv);
                    o.CsvPath = Path.GetFullPath(csv); break;

                default:
                    throw new ArgumentException($"Unknown parameter: {arg}");
            }
        }
        return o;
    }

    private static void RequireValue(string[] args, int i, string flag, out string value)
    {
        if (i >= args.Length) throw new ArgumentException($"Missing value after {flag}.");
        value = args[i];
    }
}
