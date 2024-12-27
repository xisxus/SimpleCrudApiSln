using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleCrudApi.Models;

namespace SimpleCrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MemberTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MemberTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberType>>> GetMemberTypes()
        {
            return await _context.MemberTypes.ToListAsync();
        }

        // GET: api/MemberTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberType>> GetMemberType(int id)
        {
            var memberType = await _context.MemberTypes.FindAsync(id);

            if (memberType == null)
            {
                return NotFound();
            }

            return memberType;
        }

        // PUT: api/MemberTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMemberType(int id, MemberType memberType)
        {
            if (id != memberType.MemberTypeId)
            {
                return BadRequest();
            }

            _context.Entry(memberType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberTypeExists(id))
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

        // POST: api/MemberTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MemberType>> PostMemberType(MemberType memberType)
        {
            _context.MemberTypes.Add(memberType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMemberType", new { id = memberType.MemberTypeId }, memberType);
        }

        // DELETE: api/MemberTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMemberType(int id)
        {
            var memberType = await _context.MemberTypes.FindAsync(id);
            if (memberType == null)
            {
                return NotFound();
            }

            _context.MemberTypes.Remove(memberType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberTypeExists(int id)
        {
            return _context.MemberTypes.Any(e => e.MemberTypeId == id);
        }
    }
}
