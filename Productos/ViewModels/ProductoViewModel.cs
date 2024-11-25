using Productos.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Productos.ViewModels
{
    public class ProductoViewModel : INotifyPropertyChanged
    {
        private readonly ProductosDBContext _dbContext;

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

        public ProductoViewModel(ProductosDBContext dbContext)
        {
            _dbContext = dbContext;

            AgregarProductoCommand = new Command(AgregarProducto);
            EditarProductoCommand = new Command(EditarProducto);
            EliminarProductoCommand = new Command(EliminarProducto);

            CargarCategorias();
            CargarProductos();
        }

        private void CargarProductos()
        {
            var productos = _dbContext.Productos.ToList();
            Productos = new ObservableCollection<Producto>(productos);
            OnPropertyChanged(nameof(Productos));
        }

        private void CargarCategorias()
        {
            var categorias = _dbContext.Categorias.ToList();
            Categorias = new ObservableCollection<Categoria>(categorias);
            OnPropertyChanged(nameof(Categorias));
        }

        private void AgregarProducto()
        {
            if (Categorias.Any()) // Verifica que existan categorías disponibles.
            {
                var nuevoProducto = new Producto
                {
                    Nombre = "Nuevo Producto",
                    Descripcion = "Descripción del producto",
                    Precio = 100,
                    CategoriaId = Categorias.First().Id // Asignar a la primera categoría disponible.
                };

                _dbContext.Productos.Add(nuevoProducto);
                _dbContext.SaveChanges();

                Productos.Add(nuevoProducto);
            }
        }

        private void EditarProducto()
        {
            if (ProductoSeleccionado != null)
            {
                var producto = _dbContext.Productos.Find(ProductoSeleccionado.Id);
                if (producto != null)
                {
                    producto.Nombre = ProductoSeleccionado.Nombre;
                    producto.Descripcion = ProductoSeleccionado.Descripcion;
                    producto.Precio = ProductoSeleccionado.Precio;
                    producto.CategoriaId = ProductoSeleccionado.CategoriaId;

                    _dbContext.SaveChanges();
                    CargarProductos(); // Refrescar la lista.
                }
            }
        }

        private void EliminarProducto()
        {
            if (ProductoSeleccionado != null)
            {
                var producto = _dbContext.Productos.Find(ProductoSeleccionado.Id);
                if (producto != null)
                {
                    _dbContext.Productos.Remove(producto);
                    _dbContext.SaveChanges();

                    Productos.Remove(ProductoSeleccionado);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
