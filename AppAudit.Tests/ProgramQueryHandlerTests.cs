using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppAudit.Application.Abstractions;
using AppAudit.Application.Programs.Queries;
using AppAudit.Contracts.Programs.Queries;
using Moq;
using NUnit.Framework;

namespace AppAudit.Tests;

public class ProgramQueryHandlerTests
{
    [Test]
    public async Task Handle_Returns_Programs_From_Db()
    {
        var programs = new List<ProgramEntry>
        {
            new()
            {
                ProgramId = Guid.NewGuid(),
                Name = "Test",
                DiscoveredAt = DateTimeOffset.UtcNow
            }
        }.AsQueryable();

        var mockDb = new Mock<IAppDbContext>();
        mockDb.Setup(db => db.Programs).Returns(programs);

        var handler = new GetProgramsQueryHandler(mockDb.Object);

        var result = await handler.Handle(new GetProgramsQuery(), CancellationToken.None);

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("Test", result[0].Name);
        mockDb.Verify(db => db.Programs, Times.Once);
    }
}
