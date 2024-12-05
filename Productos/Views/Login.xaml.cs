using Productos.ViewModels;

namespace Productos.Views;

public partial class Login : ContentPage
{
    private readonly EmpleadoViewModel ViewModel;
    public Login()
	{
		InitializeComponent();
        ViewModel = new EmpleadoViewModel();
        BindingContext = ViewModel;
	}

    //private async void OnLoginButtonClicked(object sender, EventArgs e)
    //{
    //    // Aquí puedes manejar la lógica de validación de login
    //    var username = usernameEntry.Text;
    //    var password = passwordEntry.Text;

    //    // Ejemplo de autenticación (esto puedes modificarlo según tu lógica)
    //    if (username == "admin" && password == "1234")
    //    {
    //        // Mostrar un mensaje de éxito
    //        await DisplayAlert("Login", "Inicio de sesión exitoso", "OK");

    //        // Cambiar la página raíz a AppShell
    //        ((App)Application.Current).ChangeToAppShell();
    //    }
    //    else
    //    {
    //        // Mostrar un mensaje de error si el login es incorrecto
    //        await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
    //    }
    //}
}