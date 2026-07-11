using MediatR;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Tasks.Dtos;
using TaskManager.Application.Tasks.Extensions;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Application.Tasks.Queries.GetTaskById;

public sealed class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
{
    private readonly ITaskRepository _tasks;
    private readonly ICurrentUserService _currentUser;

    public GetTaskByIdQueryHandler(ITaskRepository tasks, ICurrentUserService currentUser)
    {
        _tasks = tasks;
        _currentUser = currentUser;
    }

    public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _tasks.GetByIdAsync(request.Id, _currentUser.UserId, cancellationToken)
            ?? throw new NotFoundException($"Task {request.Id} not found.");

        return task.ToDto();
    }
}
