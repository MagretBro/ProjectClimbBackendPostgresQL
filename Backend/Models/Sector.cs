using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Sector
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Describe { get; set; }
        public string? NumSector { get; set; }
        public ICollection<Picture>? Pictures { get; set; } = new List<Picture>();// Массив изображений
        public string? MapPoint { get; set; } // Координаты на карте
        public Guid MassiveId { get; set; } // Внешний ключ
        public Massive? Massive { get; set; } // Навигационное свойство
        public ICollection<ClimbingRoute>? ClimbingRoutes { get; set; } = new List<ClimbingRoute>(); // Навигационное свойство
    }
}