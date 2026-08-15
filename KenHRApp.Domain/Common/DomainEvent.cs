namespace KenHRApp.Domain.Common;

public abstract record DomainEvent(DateTimeOffset OccurredOn)
{
    protected DomainEvent() : this(DateTimeOffset.UtcNow) { }
}
