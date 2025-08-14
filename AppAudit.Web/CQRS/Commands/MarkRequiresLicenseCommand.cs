using MediatR;

namespace AppAudit.Web.CQRS.Commands;

public sealed record MarkRequiresLicenseCommand(string ProgramId, bool Requires) : IRequest;
