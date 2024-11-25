using Productos.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Productos.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Nombre { get; set; }

        [MaxLength(250)]
        public string Descripcion { get; set; }

        [Required]
        public decimal Precio { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        // Propiedad de navegación para la categoría
        public Categoria Categoria { get; set; }
    }
}
