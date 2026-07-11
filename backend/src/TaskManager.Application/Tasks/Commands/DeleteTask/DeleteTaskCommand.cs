using MediatR;

namespace TaskManager.Application.Tasks.Commands.DeleteTask;

public sealed record DeleteTaskCommand(Guid Id) : IRequest;
