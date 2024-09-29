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
        
    }
}