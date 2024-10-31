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