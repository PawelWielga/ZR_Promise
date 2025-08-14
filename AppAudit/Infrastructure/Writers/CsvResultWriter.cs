using AppAudit.Core.Helpers;
using AppAudit.Core.Models;
using System.Text;

namespace AppAudit.Infrastructure.Writers;

internal sealed class CsvResultWriter(string path) : IResultWriter
{
    private readonly string _path = Path.GetFullPath(path);

    public void WriteAll(IEnumerable<ProgramEntry> items, DateTimeOffset eventUtc, string scanId)
    {
        var dir = Path.GetDirectoryName(_path);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

        var writeHeader = !File.Exists(_path) || new FileInfo(_path).Length == 0;
        using var stream = new FileStream(_path, FileMode.Append, FileAccess.Write, FileShare.Read);
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
                CsvUtils.Escape(ts),
                CsvUtils.Escape(scanId),
                CsvUtils.Escape(machine),
                CsvUtils.Escape(e.ProgramId),
                CsvUtils.Escape(e.DisplayName),
                CsvUtils.Escape(e.DisplayVersion),
                CsvUtils.Escape(e.Publisher),
                CsvUtils.Escape(e.InstallDate),
                CsvUtils.Escape(e.Architecture),
                CsvUtils.Escape(e.RegistryHive),
                CsvUtils.Escape(e.RegistryView),
                CsvUtils.Escape(e.SubkeyPath),
                CsvUtils.Escape(e.UninstallString),
                CsvUtils.Escape(e.InstallLocation),
                CsvUtils.Escape(e.ProductCode),
                CsvUtils.Escape(e.InstallSource),
                CsvUtils.Escape(e.EstimatedSize?.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                CsvUtils.Escape(e.DisplayLanguage)));
        }
    }
}
