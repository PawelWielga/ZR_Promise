using System.Text;

namespace AppAudit.Contracts.Programs.Queries;

public static class ProgramQueryExtensions
{
    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, GetProgramsPageQuery r)
        where T : IProgramInfo
    {
        if (!string.IsNullOrWhiteSpace(r.Search))
        {
            var s = r.Search.Trim();
            query = query.Where(p => p.Name.Contains(s) || (p.Publisher != null && p.Publisher.Contains(s)));
        }

        if (r.RequiresKey is true)
        {
            query = query.Where(p => p.RequiresLicense);
        }

        if (r.HasKey is true)
        {
            query = query.Where(p => !string.IsNullOrWhiteSpace(p.LicenseKey));
        }
        else if (r.HasKey is false)
        {
            query = query.Where(p => string.IsNullOrWhiteSpace(p.LicenseKey));
        }

        return query;
    }

    public static string ToQueryString(this GetProgramsPageQuery r)
    {
        var sb = new StringBuilder($"?page={r.Page}&pageSize={r.PageSize}");

        if (!string.IsNullOrWhiteSpace(r.Search))
        {
            sb.Append($"&search={Uri.EscapeDataString(r.Search)}");
        }

        if (r.RequiresKey is bool requiresKey)
        {
            sb.Append($"&requiresKey={(requiresKey ? "true" : "false")}");
        }

        if (r.HasKey is bool hasKey)
        {
            sb.Append($"&hasKey={(hasKey ? "true" : "false")}");
        }

        return sb.ToString();
    }
}
