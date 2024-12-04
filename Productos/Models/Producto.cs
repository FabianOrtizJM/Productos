using Productos.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Productos.Models
{
    public class Producto
    {
        [Key]
        public int id { get; set; }

        [MaxLength(100)]
        [Required]
        public string name { get; set; }

        [MaxLength(250)]
        public string description { get; set; }

        [Required]
        public string price { get; set; }

        [Required]
        public int categoryId { get; set; }

        // Propiedad de navegación para la categoría
        public Categoria Category { get; set; }
    }
}
