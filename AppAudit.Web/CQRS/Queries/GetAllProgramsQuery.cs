using AppAudit.Web.Models;
using MediatR;

namespace AppAudit.Web.CQRS.Queries;

public sealed record GetAllProgramsQuery() : IRequest<IReadOnlyList<ProgramDto>>;
