namespace Productos.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    private void OnLoginButtonClicked(object sender, EventArgs e)
    {
        // Aqu� puedes manejar la l�gica del login
        DisplayAlert("Login", "Inicio de sesi�n exitoso", "OK");
    }
}