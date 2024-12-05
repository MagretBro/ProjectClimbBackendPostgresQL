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
using Microsoft.AspNetCore.Authorization; 


namespace Backend.Controllers
{
    [Authorize]
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


        [AllowAnonymous] // Разрешает доступ без авторизации
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sector>>> GetAllSectors()
        {
            var sectors = await _context.Sectors
                .Include(x => x.ClimbingRoutes)
                .ToListAsync();
            return Ok(sectors);
        }

    
        // Получить sector by id
        [AllowAnonymous] // Разрешает доступ без авторизации
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
                resSector.NumSector = sector.NumSector;
                resSector.Id = sector.Id;
                resSector.Name = sector.Name;
                resSector.Describe = sector.Describe;
                resSector.Pictures = new List<Picture>();
                resSector.MapPoint = sector.MapPoint;
                resSector.ClimbingRoutes = new List<ClimbingRoute>(); 
                resSector.MassiveId = sector.MassiveId;
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
                    resClimbingRoute.NumRouter = climbingroute.NumRouter;
                    resClimbingRoute.Type = climbingroute.Type;
                    resClimbingRoute.Height = climbingroute.Height;
                    resClimbingRoute.Bolts = climbingroute.Bolts;

                }
                resSector.ClimbingRoutes.Add(resClimbingRoute);
            }
                
            return resSector; // Вернется null, если sector не найден
        }

        


///////////////
    [AllowAnonymous] // Разрешает доступ без авторизации
    [HttpGet("{sectorId}/routeCountsForAllSectorsByCategory")]
        public async Task<ActionResult<Dictionary<string, int>>> GetRouteCountsForAllSectorsByCategory(Guid sectorId) 
        {

            var sector = await _context.Sectors
                .Include(s => s.ClimbingRoutes)
                .FirstOrDefaultAsync(s => s.Id == sectorId);
            
            if (sector == null)
            {
                return NotFound($"Sector with Id '{sectorId}' not found.");
            }

                var categoryCounts = new Dictionary<string, int>{
                    {"5a", 0}, {"5b", 0}, {"5c", 0},
                    {"6a", 0}, {"6b", 0}, {"6c", 0},
                    {"7a", 0}, {"7b", 0}, {"7c", 0},
                    {"8a", 0}, {"8b", 0}
                };
                
                    foreach (var route in sector.ClimbingRoutes )
                    {
                        var category = route.Category?.Trim() ?? "";
                        if(category.EndsWith("+"))
                        {
                            category = category.Substring(0, category.Length - 1);
                        }

                        if (categoryCounts.ContainsKey(category))
                        {
                            categoryCounts[category]++;
                        }
                        else if (category.StartsWith("8") || category.StartsWith("9") || category.StartsWith("10"))
                        {
                            categoryCounts["8b"]++;
                        }
                    }
                
                return categoryCounts;

        }





        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Sector>> PostSector(Sector sector)
        {
            if (sector == null)
            {
                return BadRequest("Sector data is null.");
            }

            sector.Id = Guid.NewGuid();
            _context.Sectors.Add(sector);

            try {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            return CreatedAtAction(nameof(GetSectorById), new { Id = sector.Id}, sector);
        }

    




////////////////



   
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
        [Authorize(Roles = "Admin")] // Только для администраторов
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