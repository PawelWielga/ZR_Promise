namespace AppAudit.Contracts;

public sealed record ProgramRecord(
    Guid ProgramId,
    string Name,
    string? Version,
    string? Publisher,
    DateTimeOffset DiscoveredAt,
    bool RequiresLicense,
    string? LicenseKey
);
