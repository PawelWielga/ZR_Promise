using AppAudit.Core.Abstractions;
using AppAudit.Core.Models;
using System.Text;

namespace AppAudit.Infrastructure;

internal sealed class CsvResultWriter : IResultWriter
{
    public void WriteAll(IEnumerable<ProgramEntry> items, string path, DateTimeOffset eventUtc, string scanId)
    {
        var full = Path.GetFullPath(path);
        var dir = Path.GetDirectoryName(full);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

        var writeHeader = !File.Exists(full) || new FileInfo(full).Length == 0;
        using var stream = new FileStream(full, FileMode.Append, FileAccess.Write, FileShare.Read);
        using var writer = new StreamWriter(stream, new UTF8Encoding(false));

        if (writeHeader)
            writer.WriteLine(string.Join(',',
                "event_timestamp_utc", "scan_id",
                "machine_name", "program_id", "display_name", "display_version", "publisher", "install_date",
                "architecture", "registry_hive", "registry_view", "subkey_path", "uninstall_string",
                "install_location", "product_code", "install_source", "estimated_size", "display_language"));

        var machine = Environment.MachineName;
        var ts = eventUtc.ToString("o");
        foreach (var e in items)
        {
            writer.WriteLine(string.Join(',',
                Q(ts), Q(scanId),
                Q(machine), Q(e.ProgramId), Q(e.DisplayName), Q(e.DisplayVersion), Q(e.Publisher), Q(e.InstallDate),
                Q(e.Architecture), Q(e.RegistryHive), Q(e.RegistryView), Q(e.SubkeyPath), Q(e.UninstallString),
                Q(e.InstallLocation), Q(e.ProductCode), Q(e.InstallSource),
                Q(e.EstimatedSize?.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                Q(e.DisplayLanguage)));
        }
    }

    private static string Q(string? s)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;
        var need = s.IndexOfAny([',', '"', '\n', '\r']) >= 0;
        return need ? "\"" + s.Replace("\"", "\"\"") + "\"" : s;
    }
}
