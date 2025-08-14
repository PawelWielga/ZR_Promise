using System.Text;

namespace AppAudit.Cli
{
    internal static class Usage
    {
        public static void Print()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Usage:");
            sb.AppendLine("  AppAudit.exe [options]");
            sb.AppendLine("Options:");
            sb.AppendLine("  --interval-minutes <minutes>    Interval between scans (default: 1)");
            sb.AppendLine("  --service                       Run as Windows Service");
            sb.AppendLine("  --once                          Run only one scan and exit");
            sb.AppendLine("  --help                          Show this help");
            sb.AppendLine();
            sb.AppendLine("Service mode:");
            sb.AppendLine("  To install as a service:");
            sb.AppendLine("    sc create \"AppAudit\" binPath= \"<full-path-to-AppAudit.exe> --service\"");
            sb.AppendLine("  To start the service:");
            sb.AppendLine("    sc start AppAudit");
            sb.AppendLine("  To stop the service:");
            sb.AppendLine("    sc stop AppAudit");
            sb.AppendLine("  To delete the service:");
            sb.AppendLine("    sc delete AppAudit");

            Console.WriteLine(sb.ToString());
        }
    }
}
