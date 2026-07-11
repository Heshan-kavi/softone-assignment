namespace TaskManager.Api.Contracts;

public sealed record ChangeStatusRequest(TaskManager.Domain.Tasks.TaskStatus Status);
