using Siniestros.Domain.Entities;

namespace Siniestros.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    }
}