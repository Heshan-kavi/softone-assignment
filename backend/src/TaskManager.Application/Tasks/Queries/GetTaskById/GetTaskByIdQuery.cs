using MediatR;
using TaskManager.Application.Tasks.Dtos;

namespace TaskManager.Application.Tasks.Queries.GetTaskById;

public sealed record GetTaskByIdQuery(Guid Id) : IRequest<TaskDto>;
