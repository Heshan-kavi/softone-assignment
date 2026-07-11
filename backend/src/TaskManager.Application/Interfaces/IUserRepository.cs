using TaskManager.Domain.Users;

namespace TaskManager.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
}
