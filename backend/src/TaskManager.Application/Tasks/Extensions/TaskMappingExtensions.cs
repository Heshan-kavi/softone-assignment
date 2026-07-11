using TaskManager.Application.Tasks.Dtos;
using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Tasks.Extensions;

public static class TaskMappingExtensions
{
    public static TaskDto ToDto(this TaskItem task) => new(
        task.Id,
        task.Title,
        task.Description,
        task.Priority,
        task.Status,
        task.IsCompleted,
        task.DueDate,
        task.CreatedAt,
        task.UpdatedAt);
}
