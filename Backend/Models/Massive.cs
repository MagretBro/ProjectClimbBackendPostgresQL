using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Massive
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Describe { get; set; }
        public string[]? Picture { get; set; } // Массив изображений
        public string? MapPoint { get; set; } // Координаты на карте
        // Внешний ключ для связи с Region
        public Guid RegionId { get; set; }
        // Навигационное свойство для связи с Region
        public Region? Region { get; set; }
        public ICollection<Sector> Sectors { get; set; } = new List<Sector>(); // Навигационное свойство
    
    }
}