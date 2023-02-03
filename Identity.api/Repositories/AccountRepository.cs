using Data.Entities;
using Data.Model;
using Identity.api.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.api.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext context;

    public AccountRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<AppUser> CreateUserAsync(AppUser appUser)
    {
        var entry = await context.Users.AddAsync(appUser);
        await context.SaveChangesAsync();

        return entry.Entity;
    }

    public async Task<AppUser?> GetUserAsync(string username) 
        => await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
}
