using MediatR;

namespace AppAudit.Contracts.Programs.Queries;

public sealed record GetSummaryQuery() : IRequest<SummaryDto>;