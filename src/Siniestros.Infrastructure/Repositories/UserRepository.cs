using Microsoft.EntityFrameworkCore;
using Siniestros.Application.Interfaces;
using Siniestros.Domain.Entities;
using Siniestros.Infrastructure.Persistence;

namespace Siniestros.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SiniestroDbContext _db;

    public UserRepository(SiniestroDbContext db) => _db = db;

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
    {
        return await _db.Users
        .FirstOrDefaultAsync(x => x.Username == username);
    }
}