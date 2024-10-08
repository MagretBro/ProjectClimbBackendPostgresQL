using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class ClimbingRoute
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Describe { get; set; }
        public string? MapPoint { get; set; } // Координаты на карте в виде строки-ссылки
        public string? MapVidget { get; set; } // Виджет карты
        public string[]? Picture { get; set; } // Массив изображений
        public string? Category { get; set; }  // Категория сложности
        public string? Testimonial { get; set; } // Тип, высота, общее число шлямбуров, вкл. станцию
        public string? BoltCount { get; set; } // Разбивка на количество болтов, threads
        public Guid SectorId { get; set; } // Внешний ключ
        public Sector? Sector { get; set; } // Навигационное свойство    
    }
}