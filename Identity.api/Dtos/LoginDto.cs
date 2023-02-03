using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Identity.api.Dtos;

public class LoginDto
{
    [Required]
    [StringLength(20, MinimumLength = 5)]
    public string? UserName { get; set; }

    [Required]
    [PasswordPropertyText]
    public string? Password { get; set; }
}
