using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KioscoApp.Models
{
    public class DetalleVenta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdDetalleVenta { get; set; }

        [Required]
        public long IdVenta { get; set; }

        [Required]
        public int IdProducto { get; set; } // ¡CAMBIADO A INT!

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "numeric(10,2)")]
        public decimal PrecioUnitarioVenta { get; set; }

        [Required]
        [Column(TypeName = "numeric(10,2)")]
        public decimal Subtotal { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdVenta")]
        public Venta Venta { get; set; } = null!; // Se asume que siempre habrá un valor al cargar desde DB, o se asignará
        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; } = null!; // Se asume que siempre habrá un valor al cargar desde DB, o se asignará
    }
}