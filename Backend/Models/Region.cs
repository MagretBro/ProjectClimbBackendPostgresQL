using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Region
    {
        public Guid Id { get; set; }  
        public string? Name { get; set; }

        // Внешний ключ, который ссылается на категорию
        public Guid CountryId { get; set; }
        
        // Навигационное свойство для связи с категорией
        public Country? Country { get; set; }

        public ICollection<Massive> Massives { get; set; } = new List<Massive>(); // Навигационное свойство

    }
}