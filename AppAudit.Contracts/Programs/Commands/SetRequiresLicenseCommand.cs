using MediatR;

namespace AppAudit.Contracts.Programs.Commands;

public sealed record SetRequiresLicenseCommand(Guid ProgramId, bool Requires) : IRequest;