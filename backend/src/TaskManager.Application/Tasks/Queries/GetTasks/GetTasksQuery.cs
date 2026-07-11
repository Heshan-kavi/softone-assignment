using MediatR;
using TaskManager.Application.Tasks.Dtos;
using DomainTaskStatus = TaskManager.Domain.Tasks.TaskStatus;

namespace TaskManager.Application.Tasks.Queries.GetTasks;

public sealed record GetTasksQuery(
    DomainTaskStatus? Status = null,
    string? SortBy = null,
    string? SortOrder = null) : IRequest<IReadOnlyList<TaskDto>>;
