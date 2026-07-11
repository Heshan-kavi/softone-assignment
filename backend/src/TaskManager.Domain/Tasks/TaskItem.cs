using TaskManager.Domain.Common;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Tasks.Events;

namespace TaskManager.Domain.Tasks;

public sealed class TaskItem : Aggregate
{
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public TaskPriority Priority { get; private set; }
    public TaskStatus Status { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime? DueDate { get; private set; }
    public Guid UserId { get; private set; }

    private TaskItem() { }

    private TaskItem(Guid id, string title, string? description, TaskPriority priority, DateTime? dueDate, Guid userId)
        : base(id)
    {
        Title = title;
        Description = description;
        Priority = priority;
        Status = TaskStatus.Todo;
        IsCompleted = false;
        DueDate = dueDate;
        UserId = userId;
    }

    public static TaskItem Create(string title, string? description, TaskPriority priority, DateTime? dueDate, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title is required.");
        if (title.Length > 200)
            throw new DomainException("Title cannot exceed 200 characters.");
        if (dueDate.HasValue && dueDate.Value.Date < DateTime.UtcNow.Date)
            throw new DomainException("Due date cannot be in the past.");

        return new TaskItem(Guid.NewGuid(), title, description, priority, dueDate, userId);
    }

    public void UpdateDetails(string title, string? description, TaskPriority priority, DateTime? dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title is required.");
        if (title.Length > 200)
            throw new DomainException("Title cannot exceed 200 characters.");
        if (dueDate.HasValue && dueDate.Value.Date < DateTime.UtcNow.Date)
            throw new DomainException("Due date cannot be in the past.");

        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        Touch();
    }

    public void Complete()
    {
        if (IsCompleted)
            throw new DomainException("Task is already completed.");

        IsCompleted = true;
        Status = TaskStatus.Done;
        Touch();
        RaiseDomainEvent(new TaskCompletedDomainEvent(Id));
    }

    public void Reopen()
    {
        if (!IsCompleted)
            throw new DomainException("Task is not completed.");

        IsCompleted = false;
        Status = TaskStatus.Todo;
        Touch();
    }

    public void ChangeStatus(TaskStatus newStatus)
    {
        Status = newStatus;
        IsCompleted = newStatus == TaskStatus.Done;
        Touch();
    }
}
