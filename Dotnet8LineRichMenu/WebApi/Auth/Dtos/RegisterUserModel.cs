using System.ComponentModel.DataAnnotations;

namespace Dotnet8LineRichMenu.WebApi.Auth.Dtos;

public class RegisterUserModel
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [MaxLength(50)]
    [Required]
    public string Password { get; set; }
}