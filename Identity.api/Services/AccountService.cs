using Data.Entities;
using Data.Model;
using Identity.api.Dtos;
using Identity.api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Identity.api.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository accountRepository;
    private readonly IConfiguration configuration;

    public AccountService(IAccountRepository accountRepository, IConfiguration configuration)
    {
        this.accountRepository = accountRepository;
        this.configuration = configuration;
    }

    public async Task<Result<string>> LoginAsync(LoginDto loginDto)
    {
        var user = await accountRepository.GetUserAsync(loginDto.UserName!);
        if (user is null)
            return new(false) { ErrorMessage = "User not found" };

        if (!VerifyPasswordHash(loginDto.Password!, user.PasswordHash!, user.PasswordSalt!))
            return new(false) { ErrorMessage = "Wrong password" };

        var token = CreateToken(user);

        return new(true) { Data = token };
    }

    public async Task<Result> RegisterAsync(RegisterDto registerDto)
    {
        CreatePasswordHash(registerDto.Password!, out byte[] passwordHash, out byte[] passwordSalt);

        AppUser user = new AppUser()
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.UserName,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            TokenCreated = DateTime.UtcNow,
        };

        var userCreated = await accountRepository.CreateUserAsync(user);
        if (userCreated is null)
            return new(false) { ErrorMessage = "Can't create Accunt" };

        return new(true);
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using(var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
    
    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }

    private string CreateToken(AppUser user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            configuration.GetSection("JwtConfig:Secret").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}
