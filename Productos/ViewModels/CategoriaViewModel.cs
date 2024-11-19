using Productos.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Productos.ViewModels
{
    public class CategoriaViewModel : INotifyPropertyChanged
    {
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

        public CategoriaViewModel()
        {
            AgregarCategoriaCommand = new Command(AgregarCategoria);
            EditarCategoriaCommand = new Command(EditarCategoria);
            EliminarCategoriaCommand = new Command(EliminarCategoria);

            // Cargar datos iniciales (ejemplo)
            CargarCategorias();
        }

        private void CargarCategorias()
        {
            // Simulación de datos iniciales
            Categorias.Add(new Categoria { Id = 1, Nombre = "Electrónica", Descripcion = "Dispositivos electrónicos" });
            Categorias.Add(new Categoria { Id = 2, Nombre = "Ropa", Descripcion = "Prendas de vestir" });
        }

        private void AgregarCategoria()
        {
            // Lógica para agregar una categoría
        }

        private void EditarCategoria()
        {
            // Lógica para editar una categoría
        }

        private void EliminarCategoria()
        {
            // Lógica para eliminar una categoría
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
