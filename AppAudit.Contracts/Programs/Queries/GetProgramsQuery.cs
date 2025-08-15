using MediatR;

namespace AppAudit.Contracts.Programs.Queries;

public sealed record GetProgramsQuery() : IRequest<IReadOnlyList<ProgramRecord>>;
