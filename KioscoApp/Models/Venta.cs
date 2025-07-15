using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KioscoApp.Models
{
    public class Venta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdVenta { get; set; }

        [Required]
        public DateTime FechaVenta { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "numeric(10,2)")]
        public decimal TotalVenta { get; set; }

        [Required]
        [StringLength(50)]
        public string MetodoPago { get; set; } = string.Empty; // Inicializado

        public long? IdCliente { get; set; } // Ya es anulable

        [Required]
        [StringLength(50)]
        public string TipoComprobante { get; set; } = string.Empty; // Inicializado

        [StringLength(14)]
        public string? CAE { get; set; } // Ahora puede ser nulo

        // Propiedades de navegación
        [ForeignKey("IdCliente")]
        public Cliente? Cliente { get; set; } // Ahora puede ser nulo
        public ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>(); // Inicializada
    }
}