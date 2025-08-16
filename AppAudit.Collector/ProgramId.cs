using System.Security.Cryptography;
using System.Text;

namespace AppAudit.Collector;

public static class ProgramId
{
    static string N(string? s) => (s ?? "").Trim().ToLowerInvariant();

    public static Guid FromProduct(string name, string? version, string? publisher)
    {
        var raw = $"{N(name)}|{N(version)}|{N(publisher)}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
        Span<byte> g = stackalloc byte[16];
        hash.AsSpan(0, 16).CopyTo(g);
        g[7] = (byte)(g[7] & 0x0F | 5 << 4);
        g[8] = (byte)(g[8] & 0x3F | 0x80);
        return new Guid(g);
    }
}