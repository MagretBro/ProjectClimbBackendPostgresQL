using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.ModelsApi
{
    public class CountryApi
    {
         public Guid Id { get; set; }  // Используем Guid как тип идентификатора
        public string? Name { get; set; }

        public ICollection<RegionApi> Regions { get; set; } = new List<RegionApi>();
    }
}