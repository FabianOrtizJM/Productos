using Microsoft.EntityFrameworkCore;
using Productos.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Productos.ViewModels
{
    public class EmpleadoViewModel : INotifyPropertyChanged
    {
        private readonly ProductosDBContext _dbContext;

        public ObservableCollection<Empleado> Empleados { get; set; } = new();

        private string _nombreUsuario;
        public string NombreUsuario
        {
            get => _nombreUsuario;
            set { _nombreUsuario = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        private Empleado _nuevoEmpleado = new Empleado();
        public Empleado NuevoEmpleado
        {
            get => _nuevoEmpleado;
            set { _nuevoEmpleado = value; OnPropertyChanged(); }
        }

        public ICommand RegistrarEmpleadoCommand { get; }
        public ICommand IniciarSesionCommand { get; }
        public ICommand IrRegistroCommand { get; }
        public ICommand IrLoginCommand { get; }

        public EmpleadoViewModel()
        {

            _dbContext = new ProductosDBContext();

            IrRegistroCommand = new Command(IrRegistroView);
            IrLoginCommand = new Command(IrLoginView);
            RegistrarEmpleadoCommand = new Command(async () => RegistrarEmpleado());
            IniciarSesionCommand = new Command(async () => await IniciarSesion());

            CargarEmpleados();
        }

        private void IrRegistroView()
        {
            ((App)Application.Current).ChangeToAppRegistro();
        }

        private void IrLoginView()
        {
            ((App)Application.Current).ChangeToAppLogin();
        }

        private void CargarEmpleados()
        {
            try
            {
                var empleados = _dbContext.Empleados.ToList();
                Empleados = new ObservableCollection<Empleado>(empleados);
                OnPropertyChanged(nameof(Empleados));
            }
            catch (Exception ex)
            {
                // Manejo de errores al cargar empleados
                Application.Current.MainPage.DisplayAlert("Error", $"Error cargando empleados: {ex.Message}", "OK");
            }
        }

        private async Task RegistrarEmpleado()
        {
            if (string.IsNullOrWhiteSpace(NuevoEmpleado.Nombre) || string.IsNullOrWhiteSpace(NuevoEmpleado.Apellido) || string.IsNullOrWhiteSpace(NuevoEmpleado.Correo) || string.IsNullOrWhiteSpace(NuevoEmpleado.Password))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Favor de llenar todos los campos.", "OK");
                return;
            }

            var nuevoEmpleado = new Empleado
            {
                Nombre = NuevoEmpleado.Nombre,
                Apellido = NuevoEmpleado.Apellido,
                Correo = NuevoEmpleado.Correo,
                Password = NuevoEmpleado.Password
            };

            _dbContext.Empleados.Add(nuevoEmpleado);
            _dbContext.SaveChanges();
            Empleados.Add(nuevoEmpleado);
            NuevoEmpleado.Nombre = string.Empty;
            NuevoEmpleado.Apellido = string.Empty;
            NuevoEmpleado.Correo = string.Empty;
            NuevoEmpleado.Password = string.Empty;

            await Application.Current.MainPage.DisplayAlert("Login", "Registro exitoso", "OK");
            ((App)Application.Current).ChangeToAppLogin();
        }

        private async Task IniciarSesion()
        {
            try
            {
                var empleado = await _dbContext.Empleados
                    .FirstOrDefaultAsync(e => e.Correo == NombreUsuario && e.Password == Password);
                
                if (empleado != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Login", "Inicio de sesión exitoso", "OK");
                    ((App)Application.Current).ChangeToAppShell();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Usuario o contraseña incorrectos.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error en el inicio de sesión: {ex.Message}", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
