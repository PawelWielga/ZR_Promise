using AppAudit.Core.Models;

namespace AppAudit.Infrastructure.Deduplication;

public interface IDeduplicator
{
    IEnumerable<ProgramEntry> FilterNew(IEnumerable<ProgramEntry> items);
    void Commit(IEnumerable<ProgramEntry> written);
}
