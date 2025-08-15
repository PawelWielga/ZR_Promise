using MediatR;

namespace AppAudit.Contracts.Programs.Commands;

public sealed record SetLicenseKeyCommand(Guid ProgramId, string? LicenseKey) : IRequest;