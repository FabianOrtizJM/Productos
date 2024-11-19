using Productos.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Productos.ViewModels
{
    public class EmpleadoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Empleado> Empleados { get; set; } = new();

        private Empleado _empleadoSeleccionado;
        public Empleado EmpleadoSeleccionado
        {
            get => _empleadoSeleccionado;
            set { _empleadoSeleccionado = value; OnPropertyChanged(); }
        }

        public ICommand RegistrarEmpleadoCommand { get; }
        public ICommand IniciarSesionCommand { get; }
        public ICommand EditarEmpleadoCommand { get; }
        public ICommand EliminarEmpleadoCommand { get; }

        public EmpleadoViewModel()
        {
            RegistrarEmpleadoCommand = new Command(RegistrarEmpleado);
            IniciarSesionCommand = new Command(IniciarSesion);
            EditarEmpleadoCommand = new Command(EditarEmpleado);
            EliminarEmpleadoCommand = new Command(EliminarEmpleado);

            // Cargar datos iniciales (ejemplo)
            CargarEmpleados();
        }

        private void CargarEmpleados()
        {
            // Simulación de datos iniciales
            Empleados.Add(new Empleado { Id = 1, Nombre = "Juan", Apellido = "Perez", Correo = "juan@example.com" });
        }

        private void RegistrarEmpleado()
        {
            // Lógica para registrar un empleado
        }

        private void IniciarSesion()
        {
            // Lógica para iniciar sesión
        }

        private void EditarEmpleado()
        {
            // Lógica para editar un empleado
        }

        private void EliminarEmpleado()
        {
            // Lógica para eliminar un empleado
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
