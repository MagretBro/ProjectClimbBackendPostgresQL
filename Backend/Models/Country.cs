using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Country
    {
        public Guid Id { get; set; }  // Используем Guid как тип идентификатора
        public string? Name { get; set; }

        public ICollection<Region> Regions { get; set; } = new List<Region>();

    }
}