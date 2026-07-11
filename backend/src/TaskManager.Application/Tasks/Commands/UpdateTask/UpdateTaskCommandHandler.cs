using MediatR;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Tasks.Dtos;
using TaskManager.Application.Tasks.Extensions;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Application.Tasks.Commands.UpdateTask;

public sealed class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _tasks;
    private readonly IUnitOfWork _uow;

    public UpdateTaskCommandHandler(ITaskRepository tasks, IUnitOfWork uow)
    {
        _tasks = tasks;
        _uow = uow;
    }

    public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _tasks.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Task {request.Id} not found.");

        task.UpdateDetails(request.Title, request.Description, request.Priority, request.DueDate);
        await _uow.SaveChangesAsync(cancellationToken);
        return task.ToDto();
    }
}
