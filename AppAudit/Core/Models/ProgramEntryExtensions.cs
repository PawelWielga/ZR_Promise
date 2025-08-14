using System.Globalization;

namespace AppAudit.Core.Models;

public static class ProgramEntryExtensions
{
    public static string Fingerprint(this ProgramEntry e)
    {
        return string.Join("|",
            e.ProgramId, e.DisplayName, e.DisplayVersion, e.Publisher, e.InstallDate,
            e.Architecture, e.RegistryHive, e.RegistryView, e.SubkeyPath, e.UninstallString,
            e.InstallLocation, e.ProductCode, e.InstallSource,
            e.EstimatedSize?.ToString(CultureInfo.InvariantCulture),
            e.DisplayLanguage);
    }
}
