using AppAudit.Api.Data;
using AppAudit.Application.Abstractions;
using AppAudit.Application.Summary.Queries;
using AppAudit.Contracts.Summary.Queries;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace AppAudit.Tests;

public class GetSummaryQueryHandlerTests
{
    [Test]
    public async Task Handle_Returns_Zero_When_No_Programs()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);

        IAppDbContext db = new AppDbAdapter(context);
        var handler = new GetSummaryQueryHandler(db);

        var result = await handler.Handle(new GetSummaryQuery(), CancellationToken.None);

        Assert.AreEqual(0, result.TotalPrograms);
        Assert.AreEqual(0, result.RequiringLicense);
        Assert.AreEqual(0, result.WithoutLicenseKey);
    }

    [Test]
    public async Task Handle_Returns_Correct_Counts()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new AppDbContext(options);

        context.Programs.AddRange(
            new ProgramEntry
            {
                ProgramId = Guid.NewGuid(),
                Name = "A",
                DiscoveredAt = DateTimeOffset.UtcNow,
                RequiresLicense = false
            },
            new ProgramEntry
            {
                ProgramId = Guid.NewGuid(),
                Name = "B",
                DiscoveredAt = DateTimeOffset.UtcNow,
                RequiresLicense = true,
                LicenseKey = "KEY"
            },
            new ProgramEntry
            {
                ProgramId = Guid.NewGuid(),
                Name = "C",
                DiscoveredAt = DateTimeOffset.UtcNow,
                RequiresLicense = true,
                LicenseKey = null
            },
            new ProgramEntry
            {
                ProgramId = Guid.NewGuid(),
                Name = "D",
                DiscoveredAt = DateTimeOffset.UtcNow,
                RequiresLicense = true,
                LicenseKey = ""
            }
        );

        await context.SaveChangesAsync(CancellationToken.None);

        IAppDbContext db = new AppDbAdapter(context);
        var handler = new GetSummaryQueryHandler(db);

        var result = await handler.Handle(new GetSummaryQuery(), CancellationToken.None);

        Assert.AreEqual(4, result.TotalPrograms);
        Assert.AreEqual(3, result.RequiringLicense);
        Assert.AreEqual(2, result.WithoutLicenseKey);
    }
}
