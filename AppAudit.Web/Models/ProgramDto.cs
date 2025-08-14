using AppAudit.Core.Models;

namespace AppAudit.Web.Models;

internal sealed record ProgramDto(
string ProgramId,
string? Name,
string? Version,
string? Publisher,
bool RequiresLicense,
string? LicenseKey
)
{
    public static ProgramDto From(ProgramEntry e, bool requires, string? key) =>
        new(e.ProgramId, e.DisplayName, e.DisplayVersion, e.Publisher, requires, key);
}
