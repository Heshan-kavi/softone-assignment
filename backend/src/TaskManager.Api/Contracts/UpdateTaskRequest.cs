using TaskManager.Domain.Tasks;

namespace TaskManager.Api.Contracts;

public sealed record UpdateTaskRequest(
    string Title,
    string? Description,
    TaskPriority Priority,
    DateTime? DueDate);
