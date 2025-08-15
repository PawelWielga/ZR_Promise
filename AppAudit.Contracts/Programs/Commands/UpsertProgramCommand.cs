using MediatR;

namespace AppAudit.Contracts.Programs.Commands;

public sealed record UpsertProgramCommand(ProgramRecord Program) : IRequest;
