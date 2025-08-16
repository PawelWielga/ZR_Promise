namespace AppAudit.Contracts;

public interface IProgramInfo
{
    string Name { get; }
    string? Publisher { get; }
    bool RequiresLicense { get; }
    string? LicenseKey { get; }
}
