using AppAudit.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AppAudit.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ProgramEntry> Programs => Set<ProgramEntry>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<ProgramEntry>(e =>
        {
            e.HasKey(x => x.ProgramId);
            e.HasIndex(x => x.ProgramId).IsUnique();
            e.Property(x => x.Name).IsRequired();

            e.Property(x => x.DiscoveredAt)
             .HasConversion(
                 v => v.ToUnixTimeMilliseconds(),
                 v => DateTimeOffset.FromUnixTimeMilliseconds(v)
             );

            e.Property(x => x.Version).HasMaxLength(200);
            e.Property(x => x.Publisher).HasMaxLength(300);
            e.Property(x => x.LicenseKey).HasMaxLength(500);
        });
    }
}
