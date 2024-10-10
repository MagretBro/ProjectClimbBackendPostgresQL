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
        private readonly ILogger<SectorsController> _logger;
        private readonly AppDbContext _context;
        public SectorsController(AppDbContext context, ILogger<SectorsController> logger)
        {
            _context = context;
            _logger = logger;
        }

    
        // Получить sector by id
        [HttpGet("sector/{Id}")]
        public async Task<Sector?> GetSectorById(Guid Id)
        {
            _logger.LogInformation($"Получение сектора с ID: {Id}");

            var sector = await _context.Sectors
            .Include(x => x.ClimbingRoutes)
            .FirstOrDefaultAsync(m => m.Id == Id);

            // Если Sector не найден, возвращаем null
            if (sector == null)
            {
                _logger.LogWarning($"Сектор с ID: {Id} не найден.");
                return null;
            }
                _logger.LogInformation($"Сектор с ID: {Id} успешно найден.");


            var pictures = await _context.Pictures
                .Where(p => p.ParentId == Id)
                .ToListAsync();

            var resSector = new Sector();
            {
            resSector.Id = sector.Id;
            resSector.Name = sector.Name;
            resSector.Describe = sector.Describe;
            resSector.Pictures = new List<Picture>();
            resSector.MapPoint = sector.MapPoint;
            resSector.ClimbingRoutes = new List<ClimbingRoute>(); 
            };

            foreach (var resPic in pictures)
            {
                resSector.Pictures.Add(resPic);
            }
        
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