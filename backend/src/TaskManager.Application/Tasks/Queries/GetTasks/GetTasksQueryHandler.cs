using MediatR;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Tasks.Dtos;
using TaskManager.Application.Tasks.Extensions;

namespace TaskManager.Application.Tasks.Queries.GetTasks;

public sealed class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, IReadOnlyList<TaskDto>>
{
    private readonly ITaskRepository _tasks;

    public GetTasksQueryHandler(ITaskRepository tasks) => _tasks = tasks;

    public async Task<IReadOnlyList<TaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _tasks.ListAsync(request.Status, request.SortBy, request.SortOrder, cancellationToken);
        return tasks.Select(t => t.ToDto()).ToList().AsReadOnly();
    }
}
