using MediatR;

namespace AppAudit.Web.CQRS.Commands;

public sealed record SetLicenseKeyCommand(string ProgramId, string? LicenseKey) : IRequest;
