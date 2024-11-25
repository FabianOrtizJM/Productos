using Productos.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Productos.ViewModels
{
    public class CategoriaViewModel : INotifyPropertyChanged
    {
        private readonly ProductosDBContext _dbContext;

        public ObservableCollection<Categoria> Categorias { get; set; } = new();

        private Categoria _categoriaSeleccionada;
        public Categoria CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set { _categoriaSeleccionada = value; OnPropertyChanged(); }
        }

        public ICommand AgregarCategoriaCommand { get; }
        public ICommand EditarCategoriaCommand { get; }
        public ICommand EliminarCategoriaCommand { get; }

        public CategoriaViewModel(ProductosDBContext dbContext)
        {
            _dbContext = dbContext;

            AgregarCategoriaCommand = new Command(AgregarCategoria);
            EditarCategoriaCommand = new Command(EditarCategoria);
            EliminarCategoriaCommand = new Command(EliminarCategoria);

            CargarCategorias();
        }

        private void CargarCategorias()
        {
            var categorias = _dbContext.Categorias.ToList();
            Categorias = new ObservableCollection<Categoria>(categorias);
            OnPropertyChanged(nameof(Categorias));
        }

        private void AgregarCategoria()
        {
            var nuevaCategoria = new Categoria
            {
                Nombre = "Nueva Categoría",
                Descripcion = "Descripción de categoría"
            };
            _dbContext.Categorias.Add(nuevaCategoria);
            _dbContext.SaveChanges();

            Categorias.Add(nuevaCategoria); // Actualizamos la lista en la vista.
        }

        private void EditarCategoria()
        {
            if (CategoriaSeleccionada != null)
            {
                var categoria = _dbContext.Categorias.Find(CategoriaSeleccionada.Id);
                if (categoria != null)
                {
                    categoria.Nombre = CategoriaSeleccionada.Nombre;
                    categoria.Descripcion = CategoriaSeleccionada.Descripcion;
                    _dbContext.SaveChanges();
                    CargarCategorias(); // Refrescamos la lista.
                }
            }
        }

        private void EliminarCategoria()
        {
            if (CategoriaSeleccionada != null)
            {
                var categoria = _dbContext.Categorias.Find(CategoriaSeleccionada.Id);
                if (categoria != null)
                {
                    _dbContext.Categorias.Remove(categoria);
                    _dbContext.SaveChanges();

                    Categorias.Remove(CategoriaSeleccionada); // Actualizamos la lista.
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
