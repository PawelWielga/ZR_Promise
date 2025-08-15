using AppAudit.Contracts;

namespace AppAudit.Application.Mapping;

using Abstractions;

public static class ProgramMappings
{
    public static ProgramRecord ToDto(this ProgramEntry e) =>
        new(e.ProgramId, e.Name, e.Version, e.Publisher, e.DiscoveredAt, e.RequiresLicense, e.LicenseKey);
}
