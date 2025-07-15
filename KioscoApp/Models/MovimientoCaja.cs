using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KioscoApp.Models
{
    public class MovimientoCaja
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdMovimiento { get; set; }

        [Required]
        public DateTime FechaHora { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string TipoMovimiento { get; set; } = string.Empty; // Inicializado

        [Required]
        [Column(TypeName = "numeric(10,2)")]
        public decimal Monto { get; set; }

        public string? Descripcion { get; set; } // Ahora puede ser nulo

        [Required]
        public long IdUsuario { get; set; }

        // Propiedad de navegación
        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; } = null!; // Se asume que siempre habrá un valor
    }
}