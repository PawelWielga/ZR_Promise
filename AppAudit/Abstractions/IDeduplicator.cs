using AppAudit.Core.Models;

namespace AppAudit.Abstractions;

public interface IDeduplicator
{
    IEnumerable<ProgramEntry> FilterNew(IEnumerable<ProgramEntry> items);
    void Commit(IEnumerable<ProgramEntry> written);
}
