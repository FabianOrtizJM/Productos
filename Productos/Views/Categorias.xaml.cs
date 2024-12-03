namespace Productos.Views;

public partial class Categorias : ContentPage
{
	public Categorias()
	{
		InitializeComponent();
	}

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Navegar a la página anterior
        await Shell.Current.GoToAsync("..");
    }
}