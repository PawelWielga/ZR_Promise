using AppAudit.Web.Abstractions;
using AppAudit.Web.Models;

namespace AppAudit.Web.Infrastructure;

internal sealed class InMemoryProgramsStateStore : IProgramsStateStore
{
    private readonly Dictionary<string, ProgramState> _data = new(StringComparer.Ordinal);

    public Task<IReadOnlyDictionary<string, ProgramState>> LoadAsync(CancellationToken ct)
        => Task.FromResult<IReadOnlyDictionary<string, ProgramState>>(new Dictionary<string, ProgramState>(_data));

    public Task SaveAsync(IEnumerable<ProgramState> states, CancellationToken ct)
    {
        _data.Clear();
        foreach (var s in states) _data[s.ProgramId] = s;
        return Task.CompletedTask;
    }
}
