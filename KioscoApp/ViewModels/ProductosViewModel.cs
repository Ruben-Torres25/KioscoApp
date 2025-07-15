using System;
using System.Collections.ObjectModel; // Para ObservableCollection
using System.ComponentModel; // Para INotifyPropertyChanged
using System.Runtime.CompilerServices; // Para [CallerMemberName]
using System.Threading.Tasks; // Para Task
using System.Windows; // Solo para MessageBox temporalmente, idealmente se maneja en View
using System.Windows.Input; // Para ICommand

using KioscoApp.Models; // Para las entidades Producto
using Microsoft.EntityFrameworkCore; // Para ToListAsync, FindAsync

namespace KioscoApp.ViewModels
{
    // Hereda de BaseViewModel para la notificación de cambios de propiedades
    public class ProductosViewModel : BaseViewModel
    {
        private readonly KioscoDbContext _dbContext;

        // Colección observable para la lista de productos (se actualizará automáticamente en la UI)
        public ObservableCollection<Producto> Productos { get; set; }

        // Propiedades para los campos de entrada de un producto, con notificación de cambio
        private string _codigoBarras = string.Empty;
        public string CodigoBarras
        {
            get => _codigoBarras;
            set
            {
                // Usa SetProperty de BaseViewModel para establecer el valor y notificar cambios
                if (SetProperty(ref _codigoBarras, value))
                {
                    // Cada vez que CodigoBarras, Nombre o PrecioVenta cambian,
                    // la condición CanExecute de GuardarProductoCommand debe reevaluarse.
                    ((RelayCommand)GuardarProductoCommand).RaiseCanExecuteChanged();
                }
            }
        }

        private string _nombre = string.Empty;
        public string Nombre
        {
            get => _nombre;
            set
            {
                if (SetProperty(ref _nombre, value))
                {
                    ((RelayCommand)GuardarProductoCommand).RaiseCanExecuteChanged();
                }
            }
        }

        private decimal _precioVenta;
        public decimal PrecioVenta
        {
            get => _precioVenta;
            set
            {
                if (SetProperty(ref _precioVenta, value))
                {
                    ((RelayCommand)GuardarProductoCommand).RaiseCanExecuteChanged();
                }
            }
        }

        // Propiedad para el ID del producto en edición
        private long _productoIdEnEdicion = 0;
        public long ProductoIdEnEdicion
        {
            get => _productoIdEnEdicion;
            set => SetProperty(ref _productoIdEnEdicion, value);
        }

        // NUEVA PROPIEDAD: Producto seleccionado en el DataGrid
        private Producto? _selectedProducto;
        public Producto? SelectedProducto
        {
            get => _selectedProducto;
            set
            {
                if (SetProperty(ref _selectedProducto, value))
                {
                    // Cuando la selección cambia, cargar el producto para edición en los campos
                    CargarProductoParaEdicion(value);
                    // Esto también puede afectar si el botón de eliminar se habilita
                    ((RelayCommand)EliminarProductoCommand).RaiseCanExecuteChanged();
                }
            }
        }

        // --- Propiedades de Comandos para enlazar desde la UI ---
        public ICommand GuardarProductoCommand { get; private set; }
        public ICommand EliminarProductoCommand { get; private set; }
        public ICommand LimpiarCamposCommand { get; private set; }
        public ICommand EditarProductoCommand { get; private set; }
        // --- FIN Propiedades de Comandos ---


        // Constructor del ViewModel
        public ProductosViewModel(KioscoDbContext dbContext)
        {
            _dbContext = dbContext;
            Productos = new(); // Usando new() para simplificar
            _ = CargarProductosAsync();

            // --- Inicialización de los Comandos ---
            GuardarProductoCommand = new RelayCommand(async obj => await GuardarProductoAsync(),
                                                       obj => !string.IsNullOrWhiteSpace(CodigoBarras) &&
                                                              !string.IsNullOrWhiteSpace(Nombre) &&
                                                              PrecioVenta > 0);

            EliminarProductoCommand = new RelayCommand(async obj =>
            {
                if (obj is Producto producto)
                {
                    await EliminarProductoAsync(producto);
                }
            },
            obj => SelectedProducto != null); // CanExecute: true si hay un SelectedProducto

            LimpiarCamposCommand = new RelayCommand(obj => LimpiarCampos());

            EditarProductoCommand = new RelayCommand(obj =>
            {
                if (ProductoIdEnEdicion == 0)
                {
                    MessageBox.Show("Por favor, seleccione un producto de la lista para editar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show("Modifique los campos de arriba y luego haga clic en 'Guardar Producto'.", "Modo Edición", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            });
            // --- FIN Inicialización de los Comandos ---
        }

        // --- Métodos de Lógica de Negocio (CRUD) ---

        public async Task CargarProductosAsync()
        {
            try
            {
                var productosList = await _dbContext.Productos.ToListAsync();
                Productos.Clear();
                foreach (var p in productosList)
                {
                    Productos.Add(p);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al cargar los productos: {ex.Message}\n{ex.InnerException?.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task GuardarProductoAsync()
        {
            if (string.IsNullOrWhiteSpace(CodigoBarras) ||
                string.IsNullOrWhiteSpace(Nombre) ||
                PrecioVenta <= 0)
            {
                MessageBox.Show("Por favor, complete todos los campos y asegúrese de que el precio sea válido.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Producto producto;
                if (ProductoIdEnEdicion == 0)
                {
                    producto = new Producto
                    {
                        CodigoBarras = CodigoBarras,
                        Nombre = Nombre,
                        PrecioVenta = PrecioVenta,
                        PrecioCompra = PrecioVenta * 0.7M, // Ejemplo de cálculo
                        StockActual = 0,
                        StockMinimo = 5,
                        Activo = true,
                        Descripcion = string.Empty, // Usando string.Empty para evitar advertencia
                        Categoria = "General"
                    };
                    _dbContext.Productos.Add(producto);
                }
                else
                {
                    producto = await _dbContext.Productos.FindAsync(ProductoIdEnEdicion);
                    if (producto != null)
                    {
                        producto.CodigoBarras = CodigoBarras;
                        producto.Nombre = Nombre;
                        producto.PrecioVenta = PrecioVenta;
                        producto.PrecioCompra = PrecioVenta * 0.7M;
                    }
                }

                await _dbContext.SaveChangesAsync();

                MessageBox.Show(ProductoIdEnEdicion == 0 ? "Producto agregado exitosamente." : "Producto actualizado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                LimpiarCampos();
                await CargarProductosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al guardar el producto: {ex.Message}\n{ex.InnerException?.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task EliminarProductoAsync(Producto productoAEliminar)
        {
            if (productoAEliminar == null) return;

            MessageBoxResult result = MessageBox.Show($"¿Está seguro de que desea eliminar el producto '{productoAEliminar.Nombre}'?", "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _dbContext.Productos.Remove(productoAEliminar);
                    await _dbContext.SaveChangesAsync();

                    MessageBox.Show("Producto eliminado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    LimpiarCampos();
                    await CargarProductosAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al eliminar el producto: {ex.Message}\n{ex.InnerException?.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void LimpiarCampos()
        {
            CodigoBarras = string.Empty;
            Nombre = string.Empty;
            PrecioVenta = 0;
            ProductoIdEnEdicion = 0;
            SelectedProducto = null; // También limpia la selección del DataGrid
        }

        public void CargarProductoParaEdicion(Producto? selectedProducto)
        {
            if (selectedProducto != null)
            {
                CodigoBarras = selectedProducto.CodigoBarras;
                Nombre = selectedProducto.Nombre;
                PrecioVenta = selectedProducto.PrecioVenta;
                ProductoIdEnEdicion = selectedProducto.IdProducto;
            }
            else
            {
                LimpiarCampos();
            }
        }
    }
}