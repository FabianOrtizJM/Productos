namespace Productos.Views;

public partial class Categorias : ContentPage
{
	public Categorias()
	{
		InitializeComponent();
	}

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Navegar a la p�gina anterior
        await Shell.Current.GoToAsync("..");
    }
}