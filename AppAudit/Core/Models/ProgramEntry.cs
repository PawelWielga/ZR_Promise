namespace AppAudit.Core.Models;

public sealed record ProgramEntry(
string ProgramId,
string? DisplayName,
string? DisplayVersion,
string? Publisher,
string? InstallDate,
string Architecture,
string RegistryHive,
string RegistryView,
string SubkeyPath,
string? UninstallString,
string? InstallLocation,
string? ProductCode,
string? InstallSource,
long? EstimatedSize,
string? DisplayLanguage
);
