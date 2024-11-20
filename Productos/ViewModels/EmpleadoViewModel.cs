using Productos.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Productos.ViewModels
{
    public class EmpleadoViewModel : INotifyPropertyChanged
    {
        private readonly ProductosDBContext _dbContext;

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

        public EmpleadoViewModel(ProductosDBContext dbContext)
        {
            _dbContext = dbContext;

            RegistrarEmpleadoCommand = new Command(RegistrarEmpleado);
            IniciarSesionCommand = new Command(IniciarSesion);
            EditarEmpleadoCommand = new Command(EditarEmpleado);
            EliminarEmpleadoCommand = new Command(EliminarEmpleado);

            CargarEmpleados();
        }

        private void CargarEmpleados()
        {
            var empleados = _dbContext.Empleados.ToList();
            Empleados = new ObservableCollection<Empleado>(empleados);
            OnPropertyChanged(nameof(Empleados));
        }

        private void RegistrarEmpleado()
        {
            var nuevoEmpleado = new Empleado
            {
                Nombre = "Nuevo",
                Apellido = "Empleado",
                Correo = "nuevo@example.com",
                Password = "123456"
            };

            _dbContext.Empleados.Add(nuevoEmpleado);
            _dbContext.SaveChanges();

            Empleados.Add(nuevoEmpleado);
        }

        private void IniciarSesion()
        { }

        private void EditarEmpleado()
        {
            if (EmpleadoSeleccionado != null)
            {
                var empleado = _dbContext.Empleados.Find(EmpleadoSeleccionado.Id);
                if (empleado != null)
                {
                    empleado.Nombre = EmpleadoSeleccionado.Nombre;
                    empleado.Apellido = EmpleadoSeleccionado.Apellido;
                    empleado.Correo = EmpleadoSeleccionado.Correo;
                    empleado.Password = EmpleadoSeleccionado.Password;

                    _dbContext.SaveChanges();
                    CargarEmpleados(); // Refrescar la lista en la vista.
                }
            }
        }

        private void EliminarEmpleado()
        {
            if (EmpleadoSeleccionado != null)
            {
                var empleado = _dbContext.Empleados.Find(EmpleadoSeleccionado.Id);
                if (empleado != null)
                {
                    _dbContext.Empleados.Remove(empleado);
                    _dbContext.SaveChanges();

                    Empleados.Remove(EmpleadoSeleccionado);
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
