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
    [Authorize]  // Это ограничивает доступ ко всем методам контроллера
    [Route("api/[controller]")]
    [ApiController]
    public class ClimbingRoutesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClimbingRoutesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ClimbingRoutes
        [AllowAnonymous] // Разрешает доступ без авторизации
        [HttpGet]
        public async Task<ActionResult<List<ClimbingRoute>>> GetClimbingRoutes()
        {
            return await _context.ClimbingRoutes.Include(r => r.Sector).ToListAsync();
        }

        [AllowAnonymous] // Разрешает доступ без авторизации
        [HttpGet("{id}")]
        public async Task<ActionResult<ClimbingRoute>> GetClimbingRoute(Guid id)
        {
            var climbingroute  = await _context.ClimbingRoutes.Include(r => r.Sector)
                    .FirstOrDefaultAsync(r => r.Id == id);

            if (climbingroute == null)
            {
                return NotFound();
            }

            return climbingroute;                
        }

         [HttpPost]
         [Authorize(Roles = "Admin")]  // Только для пользователей с ролью "Admin"

        public async Task<ActionResult<ClimbingRoute>> PostClimbingRoute(ClimbingRoute climbingroute)
        {
            _context.ClimbingRoutes.Add(climbingroute);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClimbingRoute), new { id = climbingroute.Id }, climbingroute);
        }

        // PUT: api/ClimbingRoutes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClimbingRoute(Guid id, ClimbingRoute climbingroute)
        {
            if (id != climbingroute.Id)
            {
                return BadRequest();
            }

            _context.Entry(climbingroute).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClimbingRouteExists(id))
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

        // DELETE: api/ClimbingRoutes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]  // Только для администраторов

        public async Task<IActionResult> DeleteClimbingRoute(Guid id)
        {
            var climbingroute = await _context.ClimbingRoutes.FindAsync(id);
            if (climbingroute == null)
            {
                return NotFound();
            }

            _context.ClimbingRoutes.Remove(climbingroute);
            await _context.SaveChangesAsync();

            return NoContent();
        }


         private bool ClimbingRouteExists(Guid id)
        {
            return _context.ClimbingRoutes.Any(e => e.Id == id);
        }
    }
}