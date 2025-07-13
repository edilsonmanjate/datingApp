using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public required string email { get; set; } = "";
    [Required]
    public required string displayName { get; set; } = "";
    [Required]
    [MinLength(4)]
    public required string password { get; set; }


}
