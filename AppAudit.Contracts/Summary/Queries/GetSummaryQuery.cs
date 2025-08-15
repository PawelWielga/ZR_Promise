using MediatR;

namespace AppAudit.Contracts.Summary.Queries;

public sealed record GetSummaryQuery() : IRequest<SummaryDto>;