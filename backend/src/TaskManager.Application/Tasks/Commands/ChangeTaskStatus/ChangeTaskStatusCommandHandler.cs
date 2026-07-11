using MediatR;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Tasks.Dtos;
using TaskManager.Application.Tasks.Extensions;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Application.Tasks.Commands.ChangeTaskStatus;

public sealed class ChangeTaskStatusCommandHandler : IRequestHandler<ChangeTaskStatusCommand, TaskDto>
{
    private readonly ITaskRepository _tasks;
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public ChangeTaskStatusCommandHandler(ITaskRepository tasks, IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _tasks = tasks;
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<TaskDto> Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var task = await _tasks.GetByIdAsync(request.Id, _currentUser.UserId, cancellationToken)
            ?? throw new NotFoundException($"Task {request.Id} not found.");

        task.ChangeStatus(request.Status);
        await _uow.SaveChangesAsync(cancellationToken);
        return task.ToDto();
    }
}
