using AppAudit.Web.Models;
using MediatR;

namespace AppAudit.Web.CQRS.Queries;

public sealed record GetSummaryQuery() : IRequest<SummaryDto>;
