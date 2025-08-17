using AppAudit.Api.Data;
using AppAudit.Application.Abstractions;
using AppAudit.Application.Programs.Commands;
using AppAudit.Contracts.Programs.Commands;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace AppAudit.Tests;

public class SetRequiresLicenseCommandHandlerTests
{
    [Test]
    public async Task Handle_Does_Not_Disable_When_LicenseKey_Present()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);
        var program = new ProgramEntry
        {
            ProgramId = Guid.NewGuid(),
            Name = "Test",
            DiscoveredAt = DateTimeOffset.UtcNow,
            RequiresLicense = true,
            LicenseKey = "ABC"
        };
        context.Programs.Add(program);
        await context.SaveChangesAsync(CancellationToken.None);

        IAppDbContext db = new AppDbAdapter(context);
        var handler = new SetRequiresLicenseCommandHandler(db);
        var command = new SetRequiresLicenseCommand(program.ProgramId, false);

        await handler.Handle(command, CancellationToken.None);

        var updated = await context.Programs.FirstAsync(p => p.ProgramId == program.ProgramId);
        Assert.IsTrue(updated.RequiresLicense);
    }

    [Test]
    public async Task Handle_Updates_RequiresLicense_When_No_LicenseKey()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);
        var program = new ProgramEntry
        {
            ProgramId = Guid.NewGuid(),
            Name = "Test",
            DiscoveredAt = DateTimeOffset.UtcNow,
            RequiresLicense = true
        };
        context.Programs.Add(program);
        await context.SaveChangesAsync(CancellationToken.None);

        IAppDbContext db = new AppDbAdapter(context);
        var handler = new SetRequiresLicenseCommandHandler(db);
        var command = new SetRequiresLicenseCommand(program.ProgramId, false);

        await handler.Handle(command, CancellationToken.None);

        var updated = await context.Programs.FirstAsync(p => p.ProgramId == program.ProgramId);
        Assert.IsFalse(updated.RequiresLicense);
    }
}
