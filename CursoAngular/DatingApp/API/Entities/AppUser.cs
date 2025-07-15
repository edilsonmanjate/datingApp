using System.Text.Json.Serialization;
using API.Errors;

namespace API.Entities;

public class AppUser
{

    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public string? ImageUrl { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }

        // Navigation property
    [JsonIgnore]
    public Member Member { get; set; } = null!;
}
