namespace Productos.Views;

public partial class ProductosV : ContentPage
{
    public ProductoViewModel ViewModel { get; set; }
    public ProductosV()
	{
		InitializeComponent();
        ViewModel = new ProductoViewModel();
        BindingContext = ViewModel;
	}
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Navegar a la página anterior
        await Shell.Current.GoToAsync("..");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.CargarCategoriasAsync();
    }
}