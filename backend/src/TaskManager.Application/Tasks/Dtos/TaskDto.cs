using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Tasks.Dtos;

public sealed record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    TaskPriority Priority,
    Domain.Tasks.TaskStatus Status,
    bool IsCompleted,
    DateTime? DueDate,
    DateTime CreatedAt,
    DateTime UpdatedAt);
