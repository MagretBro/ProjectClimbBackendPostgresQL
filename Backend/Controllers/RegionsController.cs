using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Backend.ModelsApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization; 


namespace Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RegionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Regions
        [AllowAnonymous] // Разрешает доступ без авторизации
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Region>>> GetRegions()
        {
            return await _context.Regions.Include(r => r.Country).ToListAsync();
        }

        [AllowAnonymous] // Разрешает доступ без авторизации
        [HttpGet("{id}")]
        public async Task<ActionResult<RegionApi>> GetRegion(Guid id)
        {
            var region  = await _context.Regions.Include(r => r.Country)
                    .FirstOrDefaultAsync(r => r.CountryId == id);

            if (region == null)
            {
                return NotFound();
            }

            var regionApi = new RegionApi();
            regionApi.Id = region.Id;
            regionApi.Name = region.Name;
            regionApi.CountryId = region.CountryId;

            return regionApi;                
        }

         // POST: api/Regions
        [HttpPost]
        [Authorize(Roles = "Admin")] // Только для администраторов
        public async Task<ActionResult<Region>> PostRegion(Region region)
        {
            _context.Regions.Add(region);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRegion), new { id = region.Id }, region);
        }

        // PUT: api/Regions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegion(Guid id, Region region)
        {
            if (id != region.Id)
            {
                return BadRequest();
            }

            _context.Entry(region).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegionExists(id))
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
        
        // DELETE: api/Regions/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Только для администраторов
        public async Task<IActionResult> DeleteRegion(Guid id)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegionExists(Guid id)
        {
            return _context.Regions.Any(e => e.Id == id);
        }
    }
}