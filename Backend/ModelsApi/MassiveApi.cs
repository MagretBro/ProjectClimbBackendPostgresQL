using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.ModelsApi
{
    public class MassiveApi
    {
        
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Describe { get; set; }
        public string[]? Picture { get; set; } // Массив изображений
        public string? MapPoint { get; set; } // Координаты на карте
        // Внешний ключ для связи с Region
        public Guid RegionId { get; set; }

        public ICollection<SectorApi> Sectors { get; set; } = new List<SectorApi>(); // Навигационное свойство
    
    }
}