using AppAudit.Contracts;

namespace AppAudit.Application.Mapping;

using Abstractions;
using System.Linq.Expressions;

public static class ProgramMappings
{
    public static ProgramRecord ToDto(this ProgramEntry e) =>
        new(e.ProgramId, e.Name, e.Version, e.Publisher, e.DiscoveredAt, e.RequiresLicense, e.LicenseKey);
}

public static class ProgramMaps
{
    public static readonly Expression<Func<ProgramEntry, ProgramRecord>> ToRecord =
        p => new ProgramRecord(
            p.ProgramId,
            p.Name,
            p.Version,
            p.Publisher,
            p.DiscoveredAt,
            p.RequiresLicense,
            p.LicenseKey
        );
}