using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KioscoApp.Models
{
    public class Proveedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdProveedor { get; set; }

        [Required]
        [StringLength(255)]
        public string Nombre { get; set; } = string.Empty; // Inicializado

        [StringLength(15)]
        public string? CUIT { get; set; } // Ahora puede ser nulo

        [StringLength(255)]
        public string? DatosContacto { get; set; } // Ahora puede ser nulo

        // Propiedad de navegación inicializada
        public ICollection<Compra> Compras { get; set; } = new List<Compra>();
    }
}