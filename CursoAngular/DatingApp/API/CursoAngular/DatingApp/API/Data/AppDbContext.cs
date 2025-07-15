using API.Entities;
using API.Errors;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
     public DbSet<AppUser> Users { get; set; }
     public DbSet<Member> Members { get; set; }
     public DbSet<Photo> Photos { get; set; }

}
