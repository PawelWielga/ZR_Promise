namespace AppAudit.Console
{
    internal static class Usage
    {
        public static void Print()
        {
            System.Console.WriteLine(
                "Usage:" + Environment.NewLine +
                "  AppAudit.exe [options]" + Environment.NewLine +
                "Options:" + Environment.NewLine +
                "  --interval-minutes <minutes>" + Environment.NewLine +
                "  --service" + Environment.NewLine +
                "  --once" + Environment.NewLine +
                "  --help");
        }
    }
}
