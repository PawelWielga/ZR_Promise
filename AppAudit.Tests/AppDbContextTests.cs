using AppAudit.Api.Data;
using AppAudit.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace AppAudit.Tests;

public class AppDbContextTests
{
    [Test]
    public async Task Can_Persist_Program()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("test_db")
            .Options;

        await using var context = new AppDbContext(options);

        context.Programs.Add(new ProgramEntry
        {
            ProgramId = Guid.NewGuid(),
            Name = "Integration",
            DiscoveredAt = DateTimeOffset.UtcNow
        });

        await context.SaveChangesAsync(default);

        var list = await context.Programs.ToListAsync();

        Assert.AreEqual(1, list.Count);
        Assert.AreEqual("Integration", list[0].Name);
    }
}
