using Productos.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Productos.ViewModels
{
    public class ProductoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Producto> Productos { get; set; } = new();
        public ObservableCollection<Categoria> Categorias { get; set; } = new();

        private Producto _productoSeleccionado;
        public Producto ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set { _productoSeleccionado = value; OnPropertyChanged(); }
        }

        public ICommand AgregarProductoCommand { get; }
        public ICommand EditarProductoCommand { get; }
        public ICommand EliminarProductoCommand { get; }

        public ProductoViewModel()
        {
            AgregarProductoCommand = new Command(AgregarProducto);
            EditarProductoCommand = new Command(EditarProducto);
            EliminarProductoCommand = new Command(EliminarProducto);

            // Cargar datos iniciales (ejemplo)
            CargarProductos();
        }

        private void CargarProductos()
        {
            // Simulación de datos iniciales
            Productos.Add(new Producto { Id = 1, Nombre = "Smartphone", Precio = 500, CategoriaId = 1 });
            Productos.Add(new Producto { Id = 2, Nombre = "Camisa", Precio = 30, CategoriaId = 2 });
        }

        private void AgregarProducto()
        {
            // Lógica para agregar un producto
        }

        private void EditarProducto()
        {
            // Lógica para editar un producto
        }

        private void EliminarProducto()
        {
            // Lógica para eliminar un producto
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
