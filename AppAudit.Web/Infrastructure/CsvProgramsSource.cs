using AppAudit.Core.Models;
using AppAudit.Web.Abstractions;
using System.Globalization;

namespace AppAudit.Web.Infrastructure;

internal sealed class CsvProgramsSource(IConfiguration cfg) : IProgramsSource
{
    private readonly string _path = cfg["Audit:CsvPath"] ?? "./programs_log.csv";

    public Task<IReadOnlyList<ProgramEntry>> GetAllAsync(CancellationToken ct)
    {
        if (!File.Exists(_path)) return Task.FromResult<IReadOnlyList<ProgramEntry>>([]);
        var list = new List<ProgramEntry>(256);
        using var sr = new StreamReader(_path);
        string? line = sr.ReadLine();
        while ((line = sr.ReadLine()) is not null)
        {
            var cols = SplitCsv(line);
            if (cols.Length < 17) continue;
            var e = new ProgramEntry(
                ProgramId: cols[3],
                DisplayName: cols[4],
                DisplayVersion: cols[5],
                Publisher: cols[6],
                InstallDate: cols[7],
                Architecture: cols[8],
                RegistryHive: cols[9],
                RegistryView: cols[10],
                SubkeyPath: cols[11],
                UninstallString: cols[12],
                InstallLocation: cols[13],
                ProductCode: cols[14],
                InstallSource: cols[15],
                EstimatedSize: long.TryParse(cols[16], NumberStyles.Any, CultureInfo.InvariantCulture, out var sz) ? sz : null,
                DisplayLanguage: cols.Length > 17 ? cols[17] : null
            );
            list.Add(e);
        }
        return Task.FromResult<IReadOnlyList<ProgramEntry>>(list);

        static string[] SplitCsv(string s) => s.Split(',');
    }
}