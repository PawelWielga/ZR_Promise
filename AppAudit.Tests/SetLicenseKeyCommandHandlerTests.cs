using AppAudit.Api.Data;
using AppAudit.Application.Abstractions;
using AppAudit.Application.Programs.Commands;
using AppAudit.Contracts.Programs.Commands;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace AppAudit.Tests;

public class SetLicenseKeyCommandHandlerTests
{
    [Test]
    public async Task Handle_Sets_Trimmed_LicenseKey_And_RequiresLicense()
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
            RequiresLicense = false
        };
        context.Programs.Add(program);
        await context.SaveChangesAsync(CancellationToken.None);

        IAppDbContext db = new AppDbAdapter(context);
        var handler = new SetLicenseKeyCommandHandler(db);
        var command = new SetLicenseKeyCommand(program.ProgramId, "  ABC123  ");

        await handler.Handle(command, CancellationToken.None);

        var updated = await context.Programs.FirstAsync(p => p.ProgramId == program.ProgramId);
        Assert.AreEqual("ABC123", updated.LicenseKey);
        Assert.IsTrue(updated.RequiresLicense);
    }

    [Test]
    public async Task Handle_Clears_LicenseKey_Without_Requiring_License()
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
            RequiresLicense = false
        };
        context.Programs.Add(program);
        await context.SaveChangesAsync(CancellationToken.None);

        IAppDbContext db = new AppDbAdapter(context);
        var handler = new SetLicenseKeyCommandHandler(db);
        var command = new SetLicenseKeyCommand(program.ProgramId, " ");

        await handler.Handle(command, CancellationToken.None);

        var updated = await context.Programs.FirstAsync(p => p.ProgramId == program.ProgramId);
        Assert.IsNull(updated.LicenseKey);
        Assert.IsFalse(updated.RequiresLicense);
    }
}
