using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Picture
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string? EntityType { get; set; }
        public string? Name { get; set; }
        public string? FilePath { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public enum EntityType
    {
        ClimbingRoute,
        Sector,
        Massive
    }
}