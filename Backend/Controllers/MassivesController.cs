using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices.Marshalling;


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

        [HttpGet("{massiveId}/routeCountsByCategory")]
        public async Task<ActionResult<Dictionary<string, int>>> GetRouteCountsByCategory(Guid massiveId) 
        {

            var massive = await _context.Massives
                .Include(m => m.Sectors)
                .ThenInclude(s => s.ClimbingRoutes)
                .FirstOrDefaultAsync(m => m.Id == massiveId);
            
            if (massive == null)
            {
                return NotFound($"Massive with Id '{massiveId}' not found.");
            }

                var categoryCounts = new Dictionary<string, int>{
                    {"5a", 0}, {"5b", 0}, {"5c", 0},
                    {"6a", 0}, {"6b", 0}, {"6c", 0},
                    {"7a", 0}, {"7b", 0}, {"7c", 0},
                    {"8a", 0}, {"8b", 0}
                };
                foreach (var sector in massive.Sectors)
                {
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
                }
                return categoryCounts;

        }
    

         // Получить все массивы
        [HttpGet]
        public async Task<List<Massive>> GetMassives()
        {
            return await _context.Massives.ToListAsync();
        }

        // Получить массивы для конкретного региона
        [HttpGet("region/{regionId}")]
        public async Task<List<Massive>> GetMassivesByRegion(Guid regionId)
        {
            return await _context.Massives
                .Where(m => m.RegionId == regionId)
                .ToListAsync();
        }

        // Получить массив by id
        [HttpGet("massive/{Id}")]
        public async Task<Massive?> GetMassiveById(Guid Id)
        {
            var massive = await _context.Massives
            .Include(x => x.Sectors)
            .FirstOrDefaultAsync(m => m.Id == Id);

            // Если массив не найден, возвращаем null
            if (massive == null)
                return null;


            var resMassive = new Massive();
            {
            
            resMassive.Id = massive.Id;
            resMassive.Name = massive.Name;
            resMassive.Describe = massive.Describe;
            resMassive.Picture = massive.Picture;
            resMassive.MapPoint = massive.MapPoint;
            resMassive.Sectors = new List<Sector>(); 
    
            };
        
            foreach (var sector in massive.Sectors)
            {
                var resSector = new Sector();
                {
                    resSector.Id  = sector.Id;
                    resSector.Name = sector.Name;
                    resSector.MassiveId = sector.Id;
                    resSector.NumSector = sector.NumSector;
                    resSector.Describe = sector.Describe;
                    resSector.MapPoint = sector.MapPoint;
                }
                resMassive.Sectors.Add(resSector);
            }
                
            return resMassive; // Вернется null, если massive не найден
        }
        
    

    }
}