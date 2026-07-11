using MediatR;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Application.Tasks.Commands.DeleteTask;

public sealed class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
{
    private readonly ITaskRepository _tasks;
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public DeleteTaskCommandHandler(ITaskRepository tasks, IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _tasks = tasks;
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _tasks.GetByIdAsync(request.Id, _currentUser.UserId, cancellationToken)
            ?? throw new NotFoundException($"Task {request.Id} not found.");

        _tasks.Remove(task);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
