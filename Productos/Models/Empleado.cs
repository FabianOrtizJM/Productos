using System.ComponentModel.DataAnnotations;

namespace Productos.Models
{
    public class Empleado
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Nombre { get; set; }

        [MaxLength(50)]
        [Required]
        public string Apellido { get; set; }

        [MaxLength(100)]
        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [MaxLength(100)]
        [Required]
        public string Password { get; set; }
    }
}
