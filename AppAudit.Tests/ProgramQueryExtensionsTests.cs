using AppAudit.Contracts;
using AppAudit.Contracts.Programs.Queries;
using NUnit.Framework;

namespace AppAudit.Tests;

public class ProgramQueryExtensionsTests
{
    private static readonly List<ProgramRecord> _programs =
    [
        new(
            Guid.NewGuid(),
            "Alpha",
            "1.0",
            "Acme",
            DateTimeOffset.UtcNow,
            true,
            "KEY-1"
        ),
        new(
            Guid.NewGuid(),
            "Beta",
            "1.0",
            "BetaPub",
            DateTimeOffset.UtcNow,
            false,
            null
        ),
        new(
            Guid.NewGuid(),
            "Gamma",
            "1.0",
            "GammaPub",
            DateTimeOffset.UtcNow,
            true,
            string.Empty
        )
    ];

    [Test]
    public void ApplyFilters_Search_FiltersByNameAndPublisher()
    {
        var query = _programs.AsQueryable();
        var request = new GetProgramsPageQuery(Search: "beta");

        var result = query.ApplyFilters(request).ToList();

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo("Beta"));
    }

    [Test]
    public void ApplyFilters_RequiresKey_True_ReturnsOnlyProgramsRequiringLicense()
    {
        var query = _programs.AsQueryable();
        var request = new GetProgramsPageQuery(RequiresKey: true);

        var result = query.ApplyFilters(request).ToList();

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Select(p => p.Name), Is.EquivalentTo(new[] { "Alpha", "Gamma" }));
    }

    [Test]
    public void ApplyFilters_HasKey_True_ReturnsProgramsWithLicenseKey()
    {
        var query = _programs.AsQueryable();
        var request = new GetProgramsPageQuery(HasKey: true);

        var result = query.ApplyFilters(request).ToList();

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo("Alpha"));
    }

    [Test]
    public void ApplyFilters_HasKey_False_ReturnsProgramsWithoutLicenseKey()
    {
        var query = _programs.AsQueryable();
        var request = new GetProgramsPageQuery(HasKey: false);

        var result = query.ApplyFilters(request).ToList();

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Select(p => p.Name), Is.EquivalentTo(new[] { "Beta", "Gamma" }));
    }

    [Test]
    public void ToQueryString_BuildsCorrectStringWithAllParameters()
    {
        var request = new GetProgramsPageQuery(
            Page: 2,
            PageSize: 50,
            Search: "alpha beta",
            RequiresKey: true,
            HasKey: false);

        var result = request.ToQueryString();

        Assert.That(result, Is.EqualTo("?page=2&pageSize=50&search=alpha%20beta&requiresKey=true&hasKey=false"));
    }
}
