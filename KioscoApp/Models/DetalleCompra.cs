using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KioscoApp.Models
{
    public class DetalleCompra // ¡CORREGIDO: DEBE SER DetalleCompra!
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdDetalleCompra { get; set; }

        [Required]
        public long IdCompra { get; set; } // Clave foránea para Compra

        [Required]
        public int IdProducto { get; set; } // ¡CORRECTO: ES INT!

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "numeric(10,2)")]
        public decimal PrecioUnitarioCompra { get; set; }

        // Propiedades de navegación
        [ForeignKey("IdCompra")]
        public Compra Compra { get; set; } = null!; // Propiedad de navegación a Compra
        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; } = null!; // Propiedad de navegación a Producto
    }
}