using AppAudit.Core.Models;
using Microsoft.Win32;
using System.Collections.Concurrent;
using System.Globalization;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;

namespace AppAudit.Infrastructure;

[SupportedOSPlatform("windows")]
public sealed class ProgramScanner
{
    private static readonly (RegistryHive hive, string hiveName)[] Hives =
    [
        (RegistryHive.LocalMachine, "HKLM"),
        (RegistryHive.CurrentUser, "HKCU")
    ];

    public static IReadOnlyList<ProgramEntry> ScanAll()
    {
        var bag = new ConcurrentDictionary<string, ProgramEntry>();
        var views = new[] { RegistryView.Registry64, RegistryView.Registry32 };

        Parallel.ForEach(Hives, info =>
        {
            foreach (var view in views)
            {
                try
                {
                    using var baseKey = RegistryKey.OpenBaseKey(info.hive, view);
                    using var uninstall = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
                    if (uninstall is null) continue;

                    foreach (var subName in uninstall.GetSubKeyNames())
                    {
                        using var sub = uninstall.OpenSubKey(subName);
                        if (sub is null) continue;

                        var systemComponent = ReadInt(sub.GetValue("SystemComponent"));
                        if (systemComponent == 1) continue;

                        var dn = ReadString(sub.GetValue("DisplayName"));
                        if (string.IsNullOrWhiteSpace(dn)) continue;

                        var displayVersion = ReadString(sub.GetValue("DisplayVersion"));
                        var publisher = ReadString(sub.GetValue("Publisher"));
                        var installDate = NormalizeDate(ReadString(sub.GetValue("InstallDate")));
                        var uninstallString = ReadString(sub.GetValue("UninstallString"));
                        var installLocation = ReadString(sub.GetValue("InstallLocation"));
                        var productCode = ReadString(sub.GetValue("ProductCode"));
                        var installSource = ReadString(sub.GetValue("InstallSource"));
                        var estimatedSize = ReadLong(sub.GetValue("EstimatedSize"));
                        var displayLanguage = ReadString(sub.GetValue("DisplayLanguage")) ?? ReadString(sub.GetValue("Language"));

                        var arch = view == RegistryView.Registry64 ? "x64" : "x86";
                        var id = MakeId(info.hiveName, view, subName, dn!, displayVersion);

                        var entry = new ProgramEntry(
                            ProgramId: id,
                            DisplayName: dn,
                            DisplayVersion: displayVersion,
                            Publisher: publisher,
                            InstallDate: installDate,
                            Architecture: arch,
                            RegistryHive: info.hiveName,
                            RegistryView: arch,
                            SubkeyPath: subName,
                            UninstallString: uninstallString,
                            InstallLocation: installLocation,
                            ProductCode: productCode,
                            InstallSource: installSource,
                            EstimatedSize: estimatedSize,
                            DisplayLanguage: displayLanguage
                        );

                        bag[id] = entry;
                    }
                }
                catch (Exception ex)
                {
                    System.Console.Error.WriteLine($"[WARN] Scan error {info.hiveName} {view}: {ex.Message}");
                }
            }
        });

        return [.. bag.Values.OrderBy(e => e.DisplayName)];
    }

    private static string? ReadString(object? v) => v is string s && !string.IsNullOrWhiteSpace(s) ? s : null;

    private static int ReadInt(object? v)
    { if (v is int i) return i; if (v is long l) return (int)l; if (v is string s && int.TryParse(s, out var p)) return p; return 0; }

    private static long? ReadLong(object? v)
    { if (v is int i) return i; if (v is long l) return l; if (v is string s && long.TryParse(s, out var p)) return p; return null; }

    private static string? NormalizeDate(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;
        if (raw!.Length == 8 && DateTime.TryParseExact(raw, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt)) return dt.ToString("yyyy-MM-dd");
        if (DateTime.TryParse(raw, out var dt2)) return dt2.ToString("yyyy-MM-dd");
        return raw;
    }

    private static string MakeId(string hiveName, RegistryView view, string subKeyPath, string displayName, string? displayVersion)
    {
        var input = $"{hiveName}|{(view == RegistryView.Registry64 ? "x64" : "x86")}|{subKeyPath}|{displayName}|{displayVersion}";
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(input))).ToLowerInvariant();
    }
}