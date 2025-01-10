using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleCrudApi.Models;
using SimpleCrudApi.Models.DTOs;
using SimpleCrudApi.Services;

namespace SimpleCrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IImageHandleService _imageHandle;

        public MembersController(AppDbContext context, IImageHandleService imageHandle)
        {
            _context = context;
            _imageHandle = imageHandle;
        }

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            var members = await _context.Members
            .Include(m => m.MemberType)
            .Where(m => !m.IsDeleted)
            .ToListAsync();

            var outputModels = members.Select(m => new MemberOutputModel
            {
                MemberId = m.MemberId,
                MemberName = m.MemberName,
                MemberAddress = m.MemberAddress,
                MemberTypeName = m.MemberType.MemberTypeName,
                MemberPhoto = m.MemberPhoto,
                MemberSignature = m.MemberSignature != null ? Convert.ToBase64String(m.MemberSignature) : null
            }).ToList();

            return Ok(outputModels);
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            var member = await _context.Members
            .Include(m => m.MemberType)
            .FirstOrDefaultAsync(m => m.MemberId == id && !m.IsDeleted);

            if (member == null)
            {
                return NotFound();
            }

            var outputModel = new MemberOutputModel
            {
                MemberId = member.MemberId,
                MemberName = member.MemberName,
                MemberAddress = member.MemberAddress,
                MemberTypeName = member.MemberType.MemberTypeName,
                MemberPhoto = member.MemberPhoto,
                MemberSignature = member.MemberSignature != null ? Convert.ToBase64String(member.MemberSignature) : null
            };

            return Ok(outputModel);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(int id, MemberCreateDTOs memberDto)
        {
            if (id != memberDto.MemberId)
            {
                return BadRequest();
            }

            var existingMember = await _context.Members.FindAsync(id);
            if (existingMember == null)
            {
                return NotFound();
            }

            existingMember.MemberName = memberDto.MemberName;
            existingMember.MemberAddress = memberDto.MemberAddress;
            existingMember.MemberTypeId = (int)memberDto.MemberTypeId;

            if (memberDto.MemberPhoto != null)
            {
                existingMember.MemberPhoto = await _imageHandle.SaveFileToFolderAsync(memberDto.MemberPhoto, "");
            }

            if (memberDto.MemberSignature != null)
            {
                existingMember.MemberSignature = await _imageHandle.ConvertFileToByteArrayAsync(memberDto.MemberSignature);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<Member>> PostMember([FromForm] MemberCreateDTOs memberDto)
        {
            var photoLink = await _imageHandle.SaveFileToFolderAsync(memberDto.MemberPhoto, "");
            var signatureLink = await _imageHandle.ConvertFileToByteArrayAsync(memberDto.MemberSignature);

            Member member = new Member()
            {
                MemberName = memberDto.MemberName,
                MemberAddress = memberDto.MemberAddress,
                MemberTypeId = (int)memberDto.MemberTypeId,
                MemberSignature = signatureLink,
                MemberPhoto = photoLink,
                IsDeleted = false
            };


            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMember", new { id = member.MemberId }, member);
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }

       
    }
}
