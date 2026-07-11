using TaskManager.Domain.Common;

namespace TaskManager.Domain.Tasks.Events;

public sealed record TaskCompletedDomainEvent(Guid TaskId) : IDomainEvent;
