using AppAudit.Core.Abstractions;
using AppAudit.Core.Models;
using System.Text.Json;

namespace AppAudit.Infrastructure;

internal sealed class FileDeduplicator : IDeduplicator
{
    private readonly string _statePath;
    private readonly Dictionary<string, string> _state;
    private readonly HashSet<string> _fingerprints;

    private static JsonSerializerOptions JsonSerializerOptions => new() { WriteIndented = false };

    public FileDeduplicator(string statePath)
    {
        _statePath = Path.GetFullPath(statePath);
        var dir = Path.GetDirectoryName(_statePath);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

        _state = Load(_statePath);
        _fingerprints = new HashSet<string>(_state.Values, StringComparer.Ordinal);
    }

    public IEnumerable<ProgramEntry> FilterNew(IEnumerable<ProgramEntry> items)
    {
        foreach (var e in items)
        {
            var fp = e.Fingerprint();
            if (!_fingerprints.Contains(fp)) yield return e;
        }
    }

    public void Commit(IEnumerable<ProgramEntry> written)
    {
        foreach (var e in written)
        {
            var fp = e.Fingerprint();
            _state[e.ProgramId] = fp;
            _fingerprints.Add(fp);
        }

        var json = JsonSerializer.Serialize(_state, JsonSerializerOptions);
        var tmp = _statePath + ".tmp";
        File.WriteAllText(tmp, json);
        if (File.Exists(_statePath)) File.Replace(tmp, _statePath, null); else File.Move(tmp, _statePath);
    }

    static Dictionary<string, string> Load(string path)
    {
        try
        {
            if (!File.Exists(path)) return [];
            var txt = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(txt) ?? [];
        }
        catch { return []; }
    }
}
