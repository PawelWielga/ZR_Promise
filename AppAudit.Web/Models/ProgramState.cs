namespace AppAudit.Web.Models;

internal sealed record ProgramState(string ProgramId, bool RequiresLicense, string? LicenseKey);
