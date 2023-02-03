using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Identity.api.Dtos;

public class RegisterDto
{
    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 5)]
    public string? UserName { get; set; }

    [Required]
    [PasswordPropertyText]
    public string? Password { get; set; }

    [Required]
    [PasswordPropertyText]
    public string? ConfirmPassword { get; set; }

    public IFormFile? Avatar { get; set; }
}
