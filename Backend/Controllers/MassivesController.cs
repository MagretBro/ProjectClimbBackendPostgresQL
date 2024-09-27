using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MassivesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MassivesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Massive>>> GetMassives()
        {
            return await _context.Massives.Include(m => m.Sectors).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Massive>> GetMassive(Guid id)
        {
            var massive = await _context.Massives.Include(m => m.Sectors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (massive == null)
            {
                return NotFound();
            }

            return massive;                
        }

        [HttpPost]
        public async Task<ActionResult<Massive>> PostMassive(Massive massive)
        {
            _context.Massives.Add(massive);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMassive), new { id = massive.Id }, massive);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMassive(Guid id, Massive massive)
        {
            if (id != massive.Id)
            {
                return BadRequest();
            }

            _context.Entry(massive).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Massives.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMassive(Guid id)
        {
            var massive = await _context.Massives.FindAsync(id);
            if (massive == null)
            {
                return NotFound();
            }

            _context.Massives.Remove(massive);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
