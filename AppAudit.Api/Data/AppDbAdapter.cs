using AppAudit.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AppAudit.Api.Data;

public sealed class AppDbAdapter(AppDbContext inner) : IAppDbContext
{
    public IQueryable<ProgramEntry> Programs => inner.Programs.AsNoTracking().AsQueryable();

    public Task AddProgramAsync(ProgramEntry entity, CancellationToken ct)
    {
        inner.Programs.Add(entity);
        return Task.CompletedTask;
    }

    public async Task<ProgramEntry?> FindProgramAsync(Guid id, CancellationToken ct)
        => await inner.Programs.FirstOrDefaultAsync(p => p.ProgramId == id, ct);

    public Task<int> SaveChangesAsync(CancellationToken ct) => inner.SaveChangesAsync(ct);
}
