using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MembersController(AppDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
    {
        return await dbContext.Users.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetMember(int id)
    {
        var member = await dbContext.Users.FindAsync(id);
        if (member == null)
        {
            return NotFound();
        }
        return member;
    }

    [HttpPost]
    public IActionResult CreateMember(AppUser member)
    {
        dbContext.Users.Add(member);
        dbContext.SaveChanges();
        return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateMember(int id, AppUser member)
    {
        if (!id.Equals(member.Id))
        {
            return BadRequest();
        }
        
        dbContext.Entry(member).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        dbContext.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteMember(int id)
    {
        var member = dbContext.Users.Find(id);
        if (member == null)
        {
            return NotFound();
        }
        
        dbContext.Users.Remove(member);
        dbContext.SaveChanges();
        return NoContent();
    }

}
