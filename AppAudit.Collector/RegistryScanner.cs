using Microsoft.Win32;
using System.Runtime.Versioning;

namespace AppAudit.Collector;

[SupportedOSPlatform("windows")]
public static class RegistryScanner
{
    static readonly string[] SubKeys =
    [
        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
    @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
    ];

    public static IEnumerable<InstalledProgram> Scan()
    {
        foreach (var view in new[] { RegistryView.Registry64, RegistryView.Registry32 })
        {
            foreach (var hive in new[] { RegistryHive.LocalMachine, RegistryHive.CurrentUser })
            {
                using var baseKey = RegistryKey.OpenBaseKey(hive, view);
                foreach (var sub in SubKeys)
                {
                    using var root = baseKey.OpenSubKey(sub);
                    if (root is null) continue;

                    foreach (var name in root.GetSubKeyNames())
                    {
                        using var k = root.OpenSubKey(name);
                        if (k is null) continue;

                        var displayName = k.GetValue("DisplayName") as string;
                        if (string.IsNullOrWhiteSpace(displayName)) continue;

                        bool isSystem = GetDword(k, "SystemComponent") == 1;
                        bool noDisplay = GetDword(k, "NoDisplay") == 1;
                        string? releaseType = k.GetValue("ReleaseType") as string;
                        bool isUpdateLike = !string.IsNullOrWhiteSpace(releaseType);

                        if (isSystem || noDisplay || isUpdateLike)
                            continue;

                        var uninstall = k.GetValue("UninstallString") as string;
                        var installLoc = k.GetValue("InstallLocation") as string;

                        if (string.IsNullOrWhiteSpace(uninstall) && string.IsNullOrWhiteSpace(installLoc))
                            continue;

                        yield return new InstalledProgram
                        {
                            Name = displayName!,
                            Version = k.GetValue("DisplayVersion") as string,
                            Publisher = k.GetValue("Publisher") as string,
                            InstallLocation = installLoc,
                            UninstallString = uninstall,
                            Hive = hive,
                            View = view,
                            KeyPath = $"{sub}\\{name}"
                        };
                    }
                }
            }
        }
    }

    static int GetDword(RegistryKey k, string name)
        => k.GetValue(name) is int i ? i : 0;
}

public sealed class InstalledProgram
{
    public string Name { get; init; } = default!;
    public string? Version { get; init; }
    public string? Publisher { get; init; }
    public string? InstallLocation { get; init; }
    public string? UninstallString { get; init; }

    public RegistryHive Hive { get; init; }
    public RegistryView View { get; init; }
    public string KeyPath { get; init; } = "";
}