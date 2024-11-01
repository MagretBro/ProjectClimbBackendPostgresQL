using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.ModelsApi
{
    public class SectorApi
    {
         public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Describe { get; set; }
        public string? NumSector { get; set; }
        public string[]? Picture { get; set; } // Массив изображений
        public string? MapPoint { get; set; } // Координаты на карте
        public Guid MassiveId { get; set; } // Внешний ключ
        public ICollection<ClimbingRouteApi> ClimbingRoutes { get; set; } = new List<ClimbingRouteApi>(); // Навигационное свойство
    
    }
}