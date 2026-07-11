using TaskManager.Domain.Tasks;
using DomainTaskStatus = TaskManager.Domain.Tasks.TaskStatus;

namespace TaskManager.Application.Interfaces;

public interface ITaskRepository
{
    Task<IReadOnlyList<TaskItem>> ListAsync(Guid userId, DomainTaskStatus? status, string? sortBy, string? sortOrder, CancellationToken ct = default);
    Task<TaskItem?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default);
    void Add(TaskItem task);
    void Remove(TaskItem task);
}
