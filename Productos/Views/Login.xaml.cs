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
    //    // Aqu� puedes manejar la l�gica de validaci�n de login
    //    var username = usernameEntry.Text;
    //    var password = passwordEntry.Text;

    //    // Ejemplo de autenticaci�n (esto puedes modificarlo seg�n tu l�gica)
    //    if (username == "admin" && password == "1234")
    //    {
    //        // Mostrar un mensaje de �xito
    //        await DisplayAlert("Login", "Inicio de sesi�n exitoso", "OK");

    //        // Cambiar la p�gina ra�z a AppShell
    //        ((App)Application.Current).ChangeToAppShell();
    //    }
    //    else
    //    {
    //        // Mostrar un mensaje de error si el login es incorrecto
    //        await DisplayAlert("Error", "Usuario o contrase�a incorrectos", "OK");
    //    }
    //}
}