namespace AppAudit.Application.Abstractions;

public interface IAppDbContext
{
    IQueryable<ProgramEntry> Programs { get; }

    Task AddProgramAsync(ProgramEntry entity, CancellationToken ct);
    Task<ProgramEntry?> FindProgramAsync(Guid id, CancellationToken ct);
    Task<int> SaveChangesAsync(CancellationToken ct);
}

public sealed class ProgramEntry
{
    public Guid ProgramId { get; set; }
    public string Name { get; set; } = default!;
    public string? Version { get; set; }
    public string? Publisher { get; set; }
    public DateTimeOffset DiscoveredAt { get; set; }
    public bool RequiresLicense { get; set; }
    public string? LicenseKey { get; set; }
}