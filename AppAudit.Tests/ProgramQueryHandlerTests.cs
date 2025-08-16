using AppAudit.Api.Data;
using AppAudit.Application.Abstractions;
using AppAudit.Application.Programs.Queries;
using AppAudit.Contracts.Programs.Queries;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;

namespace AppAudit.Tests;

public class ProgramQueryHandlerTests
{
    [Test]
    public async Task Handle_Returns_Programs_From_Db()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);
        context.Programs.Add(new ProgramEntry
        {
            ProgramId = Guid.NewGuid(),
            Name = "Test",
            DiscoveredAt = DateTimeOffset.UtcNow
        });

        await context.SaveChangesAsync(CancellationToken.None);

        IAppDbContext db = new AppDbAdapter(context);
        var handler = new GetProgramsQueryHandler(db);

        var result = await handler.Handle(new GetProgramsQuery(), CancellationToken.None);

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("Test", result[0].Name);
    }
}