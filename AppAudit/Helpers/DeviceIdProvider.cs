using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AppAudit.Helpers;

internal static class DeviceIdProvider
{
    private static readonly Regex VirtualNicHint = new(
        "virtual|vmware|hyper\\-?v|vbox|loopback|bluetooth|tunnel|wireguard|zerotier|npcap",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static string? _cached;

    public static string GetDeviceId()
    {
        if (!string.IsNullOrEmpty(_cached))
            return _cached;

        var path = Path.Combine(AppContext.BaseDirectory, "device_id.txt");
        if (File.Exists(path))
        {
            _cached = File.ReadAllText(path).Trim();
            if (!string.IsNullOrEmpty(_cached))
                return _cached;
        }

        _cached = GenerateId();
        File.WriteAllText(path, _cached);
        return _cached;
    }

    private static string GenerateId()
    {
        var mac = NetworkInterface.GetAllNetworkInterfaces()
            .Where(n =>
                n.OperationalStatus == OperationalStatus.Up &&
                n.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                !VirtualNicHint.IsMatch((n.Description ?? "") + " " + (n.Name ?? "")) &&
                n.GetPhysicalAddress().GetAddressBytes().Length > 0)
            .OrderBy(n => n.NetworkInterfaceType)
            .ThenByDescending(n => n.Speed)
            .Select(n => n.GetPhysicalAddress().GetAddressBytes())
            .FirstOrDefault();

        if (mac == null || mac.Length == 0)
            mac = Encoding.UTF8.GetBytes("no-mac-" + Guid.NewGuid());

        var raw = Encoding.UTF8.GetBytes(Environment.MachineName ?? "")
            .Concat(mac)
            .ToArray();

        using var sha = SHA256.Create();
        return ToHex(sha.ComputeHash(raw));
    }

    private static string ToHex(byte[] bytes)
    {
        var c = new char[bytes.Length * 2];
        int i = 0;
        foreach (var b in bytes)
        {
            c[i++] = Nibble(b >> 4);
            c[i++] = Nibble(b & 0xF);
        }
        return new string(c);

        static char Nibble(int v) => (char)(v < 10 ? '0' + v : 'a' + (v - 10));
    }
}