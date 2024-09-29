using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.ModelsApi
{
    public class ClimbingRouteApi
    {
         public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Describe { get; set; }
        public Guid SectorId { get; set; } // Внешний ключ
        public string[]? Picture { get; set; } // Массив изображений
        public string? MapPoint { get; set; } // Координаты на карте
    
    }
}