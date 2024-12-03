using Productos.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Productos.Models
{
    public class Categoria
    {
        [Key]
        public int id { get; set; }

        [MaxLength(100)]
        [Required]
        public string? name { get; set; }

        [MaxLength(250)]
        public string? description { get; set; }

        // Propiedad de navegación para los productos en esta categoría
        public ICollection<Producto> Productos { get; set; }
    }
}
