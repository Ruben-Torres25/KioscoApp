using System.Windows;
using KioscoApp.ViewModels; // Necesario para MainViewModel

namespace KioscoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Ahora, MainWindow recibe MainViewModel a través de la inyección de dependencias
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            this.DataContext = mainViewModel; // Establece el DataContext de MainWindow al MainViewModel
        }
    }
}