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

    [Required]
    public string? Gender { get; set; } = string.Empty;

    [Required]
    public string? City { get; set; } = string.Empty;

    [Required]
    public string? Country { get; set; } = string.Empty;

        [Required]
    public DateOnly DateOfBirth { get; set; } 
    



}
