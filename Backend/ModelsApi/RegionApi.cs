using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.ModelsApi
{
    public class RegionApi
    {
         public Guid Id { get; set; }  
        public string? Name { get; set; }

        // Внешний ключ, который ссылается на категорию
        public Guid CountryId { get; set; }
        // Список массивов, который вернется в веб
        public ICollection<MassiveApi> Massives { get; set; } = new List<MassiveApi>(); // Навигационное свойство

    }
}