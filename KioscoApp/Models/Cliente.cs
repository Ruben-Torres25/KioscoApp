using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KioscoApp.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdCliente { get; set; }

        [Required]
        [StringLength(255)]
        public string Nombre { get; set; } = string.Empty; // Inicializado

        [StringLength(15)]
        public string? CUITCUIL { get; set; } // Ahora puede ser nulo

        [StringLength(255)]
        public string? Direccion { get; set; } // Ahora puede ser nulo

        [StringLength(50)]
        public string? CondicionFiscal { get; set; } // Ahora puede ser nulo

        // Propiedad de navegación inicializada
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}