using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KioscoApp.Models
{
    public class Compra
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdCompra { get; set; }

        [Required]
        public DateTime FechaCompra { get; set; } = DateTime.Now;

        [Required]
        public long IdProveedor { get; set; }

        [Required]
        [Column(TypeName = "numeric(10,2)")]
        public decimal TotalCompra { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdProveedor")]
        public Proveedor Proveedor { get; set; } = null!; // Se asume que siempre habrá un valor
        public ICollection<DetalleCompra> DetallesCompra { get; set; } = new List<DetalleCompra>(); // Inicializada
    }
}