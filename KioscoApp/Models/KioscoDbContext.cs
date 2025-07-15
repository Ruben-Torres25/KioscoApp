using Microsoft.EntityFrameworkCore;
using KioscoApp.Models; // Asegúrate de que este namespace coincida con el de tus modelos
using System.Collections.Generic; // Asegúrate de tener este using si no estaba

namespace KioscoApp.Models
{
    public class KioscoDbContext : DbContext
    {
        public KioscoDbContext(DbContextOptions<KioscoDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; } = null!;
        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Proveedor> Proveedores { get; set; } = null!;
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Venta> Ventas { get; set; } = null!;
        public DbSet<DetalleVenta> DetallesVenta { get; set; } = null!;
        public DbSet<Compra> Compras { get; set; } = null!;
        public DbSet<DetalleCompra> DetallesCompra { get; set; } = null!;
        public DbSet<MovimientoCaja> MovimientosCaja { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.CodigoBarras)
                .IsUnique();

            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.CUITCUIL)
                .IsUnique();

            modelBuilder.Entity<Proveedor>()
                .HasIndex(pr => pr.CUIT)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.NombreUsuario)
                .IsUnique();

            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Ventas)
                .HasForeignKey(v => v.IdCliente)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Venta)
                .WithMany(v => v.DetallesVenta)
                .HasForeignKey(dv => dv.IdVenta)
                .OnDelete(DeleteBehavior.Cascade);

            // ESTAS LÍNEAS DEBEN ESTAR DESCOMENTADAS AHORA:
            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Producto)
                .WithMany(p => p.DetallesVenta) // <-- DESCOMENTADO
                .HasForeignKey(dv => dv.IdProducto)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Compra>()
                .HasOne(c => c.Proveedor)
                .WithMany(pr => pr.Compras)
                .HasForeignKey(c => c.IdProveedor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetalleCompra>()
                .HasOne(dc => dc.Compra)
                .WithMany(c => c.DetallesCompra)
                .HasForeignKey(dc => dc.IdCompra)
                .OnDelete(DeleteBehavior.Cascade);

            // ESTAS LÍNEAS DEBEN ESTAR DESCOMENTADAS AHORA:
            modelBuilder.Entity<DetalleCompra>()
                .HasOne(dc => dc.Producto)
                .WithMany(p => p.DetallesCompra) // <-- DESCOMENTADO
                .HasForeignKey(dc => dc.IdProducto)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovimientoCaja>()
                .HasOne(mc => mc.Usuario)
                .WithMany(u => u.MovimientosCaja)
                .HasForeignKey(mc => mc.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}