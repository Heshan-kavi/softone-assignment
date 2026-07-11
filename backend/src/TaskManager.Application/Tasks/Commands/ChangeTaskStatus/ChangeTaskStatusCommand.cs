using MediatR;
using TaskManager.Application.Tasks.Dtos;
using DomainTaskStatus = TaskManager.Domain.Tasks.TaskStatus;

namespace TaskManager.Application.Tasks.Commands.ChangeTaskStatus;

public sealed record ChangeTaskStatusCommand(Guid Id, DomainTaskStatus Status) : IRequest<TaskDto>;
