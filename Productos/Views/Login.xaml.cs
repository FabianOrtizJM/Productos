namespace Productos.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    private void OnLoginButtonClicked(object sender, EventArgs e)
    {
        // Aquí puedes manejar la lógica del login
        DisplayAlert("Login", "Inicio de sesión exitoso", "OK");
    }
}