using Identity.api.Dtos;
using Identity.api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService accountService;

    public AccountController(IAccountService accountService)
    {
        this.accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Register is not valid");

        if (registerDto.Password != registerDto.ConfirmPassword)
            return BadRequest("Password and confirm password is wrong.");

        var result = await accountService.RegisterAsync(registerDto);
        if(!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Login is not valid");

        var result = await accountService.LoginAsync(loginDto);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        SaveCookie(result.Data);

        return Ok();
    }

    private void SaveCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
    
        HttpContext.Response.Cookies.Append("token", token, cookieOptions);
    }
}
