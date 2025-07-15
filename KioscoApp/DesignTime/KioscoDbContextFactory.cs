using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration; // Necesario para ConfigurationManager
using KioscoApp.Models; // Asegúrate de que este namespace coincida con el de tu DbContext

namespace KioscoApp
{
    // NOTA: Si creaste la carpeta DesignTime, el namespace podría ser KioscoApp.DesignTime
    // Si no, asegúrate de que coincida con el namespace de tu proyecto KioscoApp

    public class KioscoDbContextFactory : IDesignTimeDbContextFactory<KioscoDbContext>
    {
        public KioscoDbContext CreateDbContext(string[] args)
        {
            // Este método es llamado por las herramientas de EF Core en tiempo de diseño.
            // Aquí obtenemos la cadena de conexión del App.config
            string connectionString = ConfigurationManager.ConnectionStrings["KioscoDbConnection"].ConnectionString;

            var optionsBuilder = new DbContextOptionsBuilder<KioscoDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new KioscoDbContext(optionsBuilder.Options);
        }
    }
}