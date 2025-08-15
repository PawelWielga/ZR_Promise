namespace AppAudit.Contracts;

public sealed record SummaryDto(
    int TotalPrograms,
    int RequiringLicense,
    int WithoutLicenseKey
);