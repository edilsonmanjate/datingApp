using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.DTOs;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(AppDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var memberData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var members = JsonSerializer.Deserialize<List<SeedUserDto>>(memberData);

        if (members == null)
        {
            Console.WriteLine("No members in seed data");
            return;
        }

        var hmac = new HMACSHA512();

        foreach (var member in members)
        {
            var user = new AppUser
            {
                Id = member.Id,
                Email = member.Email,
                //UserName = member.Email,
                DisplayName = member.DisplayName,
                ImageUrl = member.ImageUrl,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd")),
                PasswordSalt = hmac.Key,
                Member = new Member
                {
                    Id = member.Id,
                    DisplayName = member.DisplayName,
                    Description = member.Description,
                    DateOfBirth = member.DateOfBirth,
                    ImageUrl = member.ImageUrl,
                    Gender = member.Gender,
                    City = member.City,
                    Country = member.Country,
                    LastActive = member.LastActive,
                    Created = member.Created
                }
            };

            user.Member.Photos.Add(new Photo
            {
                Url = member.ImageUrl!,
                MemberId = member.Id,
                IsApproved = true
            });

            context.Users.Add(user);

            // var result = await userManager.CreateAsync(user, "Pa$$w0rd");
            // if (!result.Succeeded)
            // {
            //     Console.WriteLine(result.Errors.First().Description);
            // }
            // await userManager.AddToRoleAsync(user, "Member");
        }

        await context.SaveChangesAsync();

        // var admin = new AppUser
        // {
        //     UserName = "admin@test.com",
        //     Email = "admin@test.com",
        //     DisplayName = "Admin"
        // };

        // await userManager.CreateAsync(admin, "Pa$$w0rd");
        // await userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);
    }

}
