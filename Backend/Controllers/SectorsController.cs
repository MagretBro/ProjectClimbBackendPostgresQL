using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectorsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SectorsController(AppDbContext context)
        {
            _context = context;
        }

    
        // Получить sector by id
        [HttpGet("sector/{Id}")]
        public async Task<Sector?> GetSectorById(Guid Id)
        {
            var sector = await _context.Sectors
            .Include(x => x.ClimbingRoutes)
            .FirstOrDefaultAsync(m => m.Id == Id);

            // Если Sector не найден, возвращаем null
            if (sector == null)
                return null;


            var resSector = new Sector();
            {
            
            resSector.Id = sector.Id;
            resSector.Name = sector.Name;
            resSector.Describe = sector.Describe;
            resSector.Picture = sector.Picture;
            resSector.MapPoint = sector.MapPoint;
            resSector.ClimbingRoutes = new List<ClimbingRoute>(); 
    
            };
        
            foreach (var climbingroute in sector.ClimbingRoutes)
            {
                var resClimbingRoute = new ClimbingRoute();
                {
                    resClimbingRoute.Id  = climbingroute.Id;
                    resClimbingRoute.Name = climbingroute.Name;
                    resClimbingRoute.Describe = climbingroute.Describe;
                    resClimbingRoute.MapPoint = climbingroute.MapPoint;
                    resClimbingRoute.MapVidget = climbingroute.MapVidget;
                    resClimbingRoute.Picture = climbingroute.Picture;
                    resClimbingRoute.Category = climbingroute.Category;
                    resClimbingRoute.Testimonial = climbingroute.Testimonial;
                    resClimbingRoute.BoltCount = climbingroute.BoltCount;

                }
                resSector.ClimbingRoutes.Add(resClimbingRoute);
            }
                
            return resSector; // Вернется null, если sector не найден


        }

        [HttpGet("sector/{sectorId}/image")]
        public async Task<IActionResult> GetSectorImage(Guid sectorId)
        {
            var sector = await _context.Sectors.FindAsync(sectorId);

            if (sector == null || sector.Picture == null)
                return NotFound();

            var imageUrl = sector.Picture[0]; // Возвращаем первый элемент массива строк
                return Ok(imageUrl); // Возвращаем путь/URL изображения
        }






        // [HttpPost]
        // public async Task<ActionResult<Sector>> PostSector(Sector sector)
        // {
        //     _context.Sectors.Add(sector);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction(nameof(GetSector), new { id = sector.Id }, sector);
        // }

        // PUT: api/Sectors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSector(Guid id, Sector sector)
        {
            if (id != sector.Id)
            {
                return BadRequest();
            }

            _context.Entry(sector).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SectorExists(id))
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

        // DELETE: api/Sectors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSector(Guid id)
        {
            var sector = await _context.Sectors.FindAsync(id);
            if (sector == null)
            {
                return NotFound();
            }

            _context.Sectors.Remove(sector);
            await _context.SaveChangesAsync();

            return NoContent();
        }


         private bool SectorExists(Guid id)
        {
            return _context.Sectors.Any(e => e.Id == id);
        }
    }
}