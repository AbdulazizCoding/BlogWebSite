using Data.Entities;
using Data.Model;

namespace Identity.api.Repositories;

public interface IAccountRepository
{
    Task<AppUser> CreateUserAsync(AppUser appUser);
    Task<AppUser> GetUserAsync(string username);
}