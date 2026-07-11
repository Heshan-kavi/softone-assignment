using MediatR;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Tasks.Dtos;
using TaskManager.Application.Tasks.Extensions;
using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Tasks.Commands.CreateTask;

public sealed class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _tasks;
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public CreateTaskCommandHandler(ITaskRepository tasks, IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _tasks = tasks;
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = TaskItem.Create(request.Title, request.Description, request.Priority, request.DueDate, _currentUser.UserId);
        _tasks.Add(task);
        await _uow.SaveChangesAsync(cancellationToken);
        return task.ToDto();
    }
}
