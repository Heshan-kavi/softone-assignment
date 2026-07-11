using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Contracts;
using TaskManager.Application.Tasks.Commands.ChangeTaskStatus;
using TaskManager.Application.Tasks.Commands.CompleteTask;
using TaskManager.Application.Tasks.Commands.CreateTask;
using TaskManager.Application.Tasks.Commands.DeleteTask;
using TaskManager.Application.Tasks.Commands.UpdateTask;
using TaskManager.Application.Tasks.Queries.GetTaskById;
using TaskManager.Application.Tasks.Queries.GetTasks;
using DomainTaskStatus = TaskManager.Domain.Tasks.TaskStatus;

namespace TaskManager.Api.Endpoints;

public static class TaskEndpoints
{
    public static IEndpointRouteBuilder MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks").WithTags("Tasks").RequireAuthorization();

        group.MapGet("/", async (
            [FromQuery] int? status,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortOrder,
            [FromServices] ISender sender,
            CancellationToken ct) =>
        {
            DomainTaskStatus? domainStatus = status.HasValue ? (DomainTaskStatus)status.Value : null;
            return Results.Ok(await sender.Send(new GetTasksQuery(domainStatus, sortBy, sortOrder), ct));
        });

        group.MapGet("/{id:guid}", async (Guid id, [FromServices] ISender sender, CancellationToken ct) =>
            Results.Ok(await sender.Send(new GetTaskByIdQuery(id), ct)));

        group.MapPost("/", async (
            [FromBody] CreateTaskCommand command,
            [FromServices] ISender sender,
            CancellationToken ct) =>
        {
            var dto = await sender.Send(command, ct);
            return Results.Created($"/api/tasks/{dto.Id}", dto);
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            [FromBody] UpdateTaskRequest body,
            [FromServices] ISender sender,
            CancellationToken ct) =>
            Results.Ok(await sender.Send(new UpdateTaskCommand(id, body.Title, body.Description, body.Priority, body.DueDate), ct)));

        group.MapPatch("/{id:guid}/complete", async (
            Guid id,
            [FromServices] ISender sender,
            CancellationToken ct) =>
            Results.Ok(await sender.Send(new CompleteTaskCommand(id), ct)));

        group.MapPatch("/{id:guid}/status", async (
            Guid id,
            [FromBody] ChangeStatusRequest body,
            [FromServices] ISender sender,
            CancellationToken ct) =>
            Results.Ok(await sender.Send(new ChangeTaskStatusCommand(id, body.Status), ct)));

        group.MapDelete("/{id:guid}", async (Guid id, [FromServices] ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new DeleteTaskCommand(id), ct);
            return Results.NoContent();
        });

        return app;
    }
}
