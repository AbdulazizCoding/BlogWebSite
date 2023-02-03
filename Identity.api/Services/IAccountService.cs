using Data.Model;
using Identity.api.Dtos;

namespace Identity.api.Services;

public interface IAccountService
{
    Task<Result> RegisterAsync(RegisterDto registerDto);
    Task<Result<string>> LoginAsync(LoginDto loginDto);
}
