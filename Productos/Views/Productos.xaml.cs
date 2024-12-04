namespace Productos.Views;

public partial class ProductosV : ContentPage
{
	public ProductosV()
	{
		InitializeComponent();
        BindingContext = new ProductoViewModel();
	}
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Navegar a la p�gina anterior
        await Shell.Current.GoToAsync("..");
    }
}