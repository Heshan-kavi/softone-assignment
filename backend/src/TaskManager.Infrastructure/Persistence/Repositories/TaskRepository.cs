using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Tasks;
using DomainTaskStatus = TaskManager.Domain.Tasks.TaskStatus;

namespace TaskManager.Infrastructure.Persistence.Repositories;

public sealed class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context) => _context = context;

    public async Task<IReadOnlyList<TaskItem>> ListAsync(DomainTaskStatus? status, string? sortBy, string? sortOrder, CancellationToken ct = default)
    {
        var query = _context.Tasks.AsQueryable();

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        var desc = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);

        query = sortBy?.ToLower() switch
        {
            "title"     => desc ? query.OrderByDescending(t => t.Title)     : query.OrderBy(t => t.Title),
            "status"    => desc ? query.OrderByDescending(t => t.Status)    : query.OrderBy(t => t.Status),
            "priority"  => desc ? query.OrderByDescending(t => t.Priority)  : query.OrderBy(t => t.Priority),
            "duedate"   => desc ? query.OrderByDescending(t => t.DueDate)   : query.OrderBy(t => t.DueDate),
            _           => desc ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt)
        };

        return await query.ToListAsync(ct);
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Tasks.FindAsync([id], ct);

    public void Add(TaskItem task) => _context.Tasks.Add(task);

    public void Remove(TaskItem task) => _context.Tasks.Remove(task);
}
