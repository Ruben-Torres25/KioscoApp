using System.Windows.Input; // Para ICommand
using KioscoApp.Models;
using KioscoApp.Views; // Para ProductosView
using KioscoApp.ViewModels; // Para ProductosViewModel

namespace KioscoApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // Propiedad que contendrá el ViewModel de la vista actual
        private BaseViewModel _currentViewModel = null!; // El '!' indica que será inicializado y no será nulo
        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        // ViewModels de las diferentes secciones de la aplicación
        public ProductosViewModel ProductosVm { get; }
        // Aquí agregarás más ViewModels en el futuro, como VentasViewModel, ClientesViewModel, etc.

        // Comandos para la navegación
        public ICommand ShowProductosViewCommand { get; }
        // Aquí agregarás más comandos para navegar a otras vistas

        public MainViewModel(ProductosViewModel productosViewModel)
        {
            ProductosVm = productosViewModel;
            CurrentViewModel = ProductosVm; // Al iniciar, muestra la vista de Productos por defecto

            ShowProductosViewCommand = new RelayCommand(ExecuteShowProductosViewCommand);
            // Inicializa aquí otros comandos de navegación
        }

        private void ExecuteShowProductosViewCommand(object? parameter) // <-- Añadir '?'
        {
            CurrentViewModel = ProductosVm;
        }

        // Métodos Execute para otros comandos de navegación irían aquí
    }
}