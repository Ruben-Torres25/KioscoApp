using System;
using System.Configuration;
using System.Windows;
using System.Globalization;
using System.Threading;

using KioscoApp.Models;
using KioscoApp.ViewModels; // Necesario para MainViewModel y ProductosViewModel
using KioscoApp.Views; // Necesario para MainWindow y ProductosView

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KioscoApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider? _serviceProvider;

        public App()
        {
            // Establecer la cultura para que la coma sea el separador decimal
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");

            ConfigureServices();
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            // Obtener la cadena de conexión del App.config
            string connectionString = ConfigurationManager.ConnectionStrings["KioscoDBConnection"].ConnectionString;

            // Registrar KioscoDbContext con Npgsql para PostgreSQL
            services.AddDbContext<KioscoDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Registrar ViewModels
            services.AddSingleton<ProductosViewModel>();
            services.AddSingleton<MainViewModel>(); // <<-- ¡REGISTRAR MAINVIEWMODEL!

            // Registrar MainWindow. Ahora MainWindow recibirá el MainViewModel
            services.AddSingleton<MainWindow>(provider => new MainWindow(
                provider.GetRequiredService<MainViewModel>())); // <<-- ¡MAINWINDOW RECIBE MAINVIEWMODEL!

            // Construir el ServiceProvider
            _serviceProvider = services.BuildServiceProvider();
        }

        // Este método se ejecuta cuando la aplicación se inicia
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Obtener la ventana principal del ServiceProvider y mostrarla
            var mainWindow = _serviceProvider!.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}