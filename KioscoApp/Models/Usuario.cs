using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KioscoApp.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdUsuario { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreUsuario { get; set; } = string.Empty; // Inicializado

        [Required]
        [StringLength(255)]
        public string ContrasenaHash { get; set; } = string.Empty; // Inicializado

        [Required]
        [StringLength(50)]
        public string Rol { get; set; } = string.Empty; // Inicializado

        [StringLength(255)]
        public string? NombreCompleto { get; set; } // Ahora puede ser nulo

        // Propiedades de navegación inicializada
        public ICollection<MovimientoCaja> MovimientosCaja { get; set; } = new List<MovimientoCaja>();
    }
}