using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extentions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class MembersController(IMemberRepository memberRepository,    IPhotoService photoService) : BaseApiController
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

    [HttpPut]
    public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto, IPhotoService photoService)
    {

        var memberId = User.GetMemberId();

        var member = await memberRepository.GetMemberForUpdate(memberId);
        if (member == null) return NotFound("Member not found");

        member.DisplayName = memberUpdateDto.DisplayName ?? member.DisplayName;
        member.Description = memberUpdateDto.Description ?? member.Description;
        member.City = memberUpdateDto.City ?? member.City;
        member.Country = memberUpdateDto.Country ?? member.Country;

        member.User.DisplayName = memberUpdateDto.DisplayName ?? member.User.DisplayName;

        memberRepository.Update(member);

        //uow.MemberRepository.Update(member); // optional

        if (await memberRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update member");
    }


    [HttpPost("add-photo")]
    public async Task<ActionResult<Photo>> AddPhoto([FromForm] IFormFile file)
    {
        var member = await memberRepository.GetMemberForUpdate(User.GetMemberId());

        if (member == null) return BadRequest("Cannot update member");

        var result = await photoService.UploadPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            MemberId = User.GetMemberId(),
            //IsApproved = true
        };

        if (member.ImageUrl == null)
        {
            member.ImageUrl = photo.Url;
            member.User.ImageUrl = photo.Url;
        }

        member.Photos.Add(photo);

        if (await memberRepository.SaveAllAsync()) return photo;

        return BadRequest("Problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var member = await memberRepository.GetMemberForUpdate(User.GetMemberId());

        if (member == null) return BadRequest("Cannot get member from token");

        var photo = member.Photos.SingleOrDefault(x => x.Id == photoId);

        if (member.ImageUrl == photo?.Url || photo == null)
        {
            return BadRequest("Cannot set this as main image");
        }

        member.ImageUrl = photo.Url;
        member.User.ImageUrl = photo.Url;

        if (await memberRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Problem setting main photo");
    }

    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var member = await memberRepository.GetMemberForUpdate(User.GetMemberId());

        if (member == null) return BadRequest("Cannot get member from token");

        var photo = member.Photos.SingleOrDefault(x => x.Id == photoId);

        if (photo == null || photo.Url == member.ImageUrl)
        {
            return BadRequest("This photo cannot be deleted");
        }

        if (photo.PublicId != null)
        {
            var result = await photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }

        member.Photos.Remove(photo);

        if (await memberRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting the photo");
    }
}

