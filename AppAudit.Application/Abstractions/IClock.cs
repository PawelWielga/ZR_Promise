namespace AppAudit.Application.Abstractions;

public interface IClock
{
    DateTimeOffset Now { get; }
}