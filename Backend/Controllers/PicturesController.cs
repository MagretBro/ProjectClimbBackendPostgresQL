using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PicturesController : ControllerBase
    {
        
    private readonly AppDbContext _dbContext;

        public PicturesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddPicture([FromBody] AddPictureRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.FilePath) || request.ParentId == Guid.Empty)
            {
                return BadRequest("Invalid data provided.");
            }

            // Создаем новую картинку
            var picture = new Picture
            {
                Id = Guid.NewGuid(),
                ParentId = request.ParentId,
                EntityType = request.EntityType.ToString(),
                Name = request.Name,
                FilePath = request.FilePath,
                CreatedAt = DateTime.UtcNow
            };

            // Добавляем в базу
            _dbContext.Pictures.Add(picture);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPictureById), new { id = picture.Id }, picture);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPictureById(Guid id)
        {
            var picture = await _dbContext.Pictures.FindAsync(id);
            if (picture == null)
            {
                return NotFound();
            }

            return Ok(picture);
        }
    }

    public class AddPictureRequest
    {
        public Guid ParentId { get; set; }
        public EntityType EntityType { get; set; }
        public string? Name { get; set; }
        public string FilePath { get; set; } = string.Empty;
    }
}