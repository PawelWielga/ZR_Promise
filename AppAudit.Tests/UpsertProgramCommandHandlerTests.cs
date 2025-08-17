using AppAudit.Api.Data;
using AppAudit.Application.Abstractions;
using AppAudit.Application.Programs.Commands;
using AppAudit.Contracts;
using AppAudit.Contracts.Programs.Commands;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace AppAudit.Tests;

public class UpsertProgramCommandHandlerTests
{
    [Test]
    public async Task Handle_Adds_Program_With_Clock_Time_When_DiscoveredAt_Default()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);
        IAppDbContext db = new AppDbAdapter(context);
        var clock = new Mock<IClock>();
        var now = new DateTimeOffset(2024, 1, 2, 3, 4, 5, TimeSpan.Zero);
        clock.Setup(c => c.Now).Returns(now);

        var handler = new UpsertProgramCommandHandler(db, clock.Object);
        var record = new ProgramRecord(Guid.NewGuid(), "Test", null, null, default, false, null);
        var command = new UpsertProgramCommand(record);

        await handler.Handle(command, CancellationToken.None);

        var added = await context.Programs.FirstAsync();
        Assert.AreEqual(now, added.DiscoveredAt);
    }

    [Test]
    public async Task Handle_Does_Not_Add_When_Program_Already_Exists()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);
        var program = new ProgramEntry
        {
            ProgramId = Guid.NewGuid(),
            Name = "Existing",
            DiscoveredAt = DateTimeOffset.UtcNow
        };
        context.Programs.Add(program);
        await context.SaveChangesAsync(CancellationToken.None);

        IAppDbContext db = new AppDbAdapter(context);
        var clock = new Mock<IClock>();
        clock.Setup(c => c.Now).Returns(DateTimeOffset.UtcNow);

        var handler = new UpsertProgramCommandHandler(db, clock.Object);
        var record = new ProgramRecord(program.ProgramId, "Existing", null, null, program.DiscoveredAt, false, null);
        var command = new UpsertProgramCommand(record);

        await handler.Handle(command, CancellationToken.None);

        var count = await context.Programs.CountAsync();
        Assert.AreEqual(1, count);
    }
}
