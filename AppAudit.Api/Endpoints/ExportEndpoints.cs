using AppAudit.Api.Data;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AppAudit.Api.Endpoints;

public static class ExportEndpoints
{
    public static IEndpointRouteBuilder MapExportEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/export.csv", async (AppDbContext db) =>
        {
            var data = await db.Programs
                .Where(p => p.RequiresLicense && (p.LicenseKey == null || p.LicenseKey == ""))
                .OrderBy(p => p.Name)
                .Select(p => new { p.Name, p.Version, p.Publisher, p.ProgramId })
                .ToListAsync();

            await using var ms = new MemoryStream();
            await using var writer = new StreamWriter(ms);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
            csv.WriteRecords(data);
            await writer.FlushAsync();
            ms.Position = 0;
            return Results.File(ms.ToArray(), "text/csv", "programs_missing_license.csv");
        });

        return app;
    }
}