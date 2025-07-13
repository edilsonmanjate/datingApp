using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class MembersController(IMemberRepository memberRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers()
    {
        return Ok(await memberRepository.GetMembersAsync());
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Member>> GetMember(string id)
    {
        var member = await memberRepository.GetMemberByIdAsync(id);
        if (member == null)
        {
            return NotFound();
        }
        return member;
    }

    [HttpGet("{id}/photos")]
    public async Task<ActionResult<IReadOnlyList<Photo>>> GetPhotosForMember(string id)
    {
        var photos = await memberRepository.GetPhotosForMemberAsync(id);
        if (photos == null || !photos.Any())
        {
            return NotFound();
        }
        return Ok(photos);
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Member>> CreateMember(Member member)
    {
        if (member == null)
        {
            return BadRequest("Member data is required.");
        }

        memberRepository.Update(member);
        if (await memberRepository.SaveAllAsync())
        {
            return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
        }
        return BadRequest("Failed to create member.");
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateMember(string id, Member member)
    {
        if (id != member.Id)
        {
            return BadRequest("Member ID mismatch.");
        }

        var existingMember = await memberRepository.GetMemberByIdAsync(id);
        if (existingMember == null)
        {
            return NotFound();
        }

        memberRepository.Update(member);
        if (await memberRepository.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Failed to update member.");
    }

    
}
