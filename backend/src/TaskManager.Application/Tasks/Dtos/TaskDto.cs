namespace TaskManager.Application.Tasks.Dtos;

public sealed record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    string Priority,
    string Status,
    bool IsCompleted,
    DateTime? DueDate,
    DateTime CreatedAt,
    DateTime UpdatedAt);
