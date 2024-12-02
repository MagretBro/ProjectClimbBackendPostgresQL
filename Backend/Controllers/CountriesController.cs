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
    public class CountriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CountriesController(AppDbContext context)
        {
            _context = context;
        }

       

        // GET: api/GetCountries
        [AllowAnonymous] // Разрешает доступ без авторизации
        [HttpGet]
        public async Task<List<Country>> GetCountries()
        {
            var nCountryList = await _context.Countries
                .Include(x => x.Regions)
                .ThenInclude(r => r.Massives)
                .ToListAsync();

            var resCountryList = new List<Country>();
            foreach (var country in nCountryList)
            {
                var resCountry = new Country();
                resCountry.Id = country.Id;
                resCountry.Name = country.Name;
                resCountry.Regions= new List<Region>();

                foreach (var region in country.Regions)
                {
                    var resRegion = new Region();
                    resRegion.Id  = region.Id;
                    resRegion.Name = region.Name;
                    resRegion.CountryId = region.CountryId;
                

                    foreach (var massive in region.Massives)
                    {
                        var resMassive = new Massive();
                        resMassive.Id  = massive.Id;
                        resMassive.Name = massive.Name;
                        resMassive.RegionId = massive.RegionId;

                        resRegion.Massives.Add(resMassive);
                    }

                    resCountry.Regions.Add(resRegion);
                }

                resCountryList.Add(resCountry);
            }

            return resCountryList;
        }




        // // GET: api/GetCountries
        // [HttpGet]
        // public async Task<ActionResult<List<Country>>> GetCountries()
        // {
        //     return await _context.Countries.ToListAsync();
        // }




        // GET: api/Countries/5
        // [HttpGet("{id}")]
        // public async Task<ActionResult<Country>> GetCountry(Guid id)
        // {
        //     var country = await _context.Countries.FindAsync(id);

        //     if (country == null)
        //     {
        //         return NotFound();
        //     }

        //     return country;
        // }

        // POST: api/Countries
        // [HttpPost]
        // public async Task<ActionResult<Country>> PostCountry(Country country)
        // {
        //     _context.Countries.Add(country);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
        // }

        // PUT: api/Countries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(Guid id, Country country)
        {
            if (id != country.Id)
            {
                return BadRequest();
            }

            _context.Entry(country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
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

        // // DELETE: api/Countries/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteCountry(Guid id)
        // {
        //     var Country = await _context.Countries.FindAsync(id);
        //     if (Country == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Countries.Remove(Country);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        private bool CountryExists(Guid id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }
    }
}