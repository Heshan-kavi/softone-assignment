using MediatR;
using TaskManager.Application.Tasks.Dtos;

namespace TaskManager.Application.Tasks.Commands.CompleteTask;

public sealed record CompleteTaskCommand(Guid Id) : IRequest<TaskDto>;
