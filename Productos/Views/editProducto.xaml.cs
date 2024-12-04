using System.Text.Json;
using Productos.Models;

namespace Productos.Views;

[QueryProperty(nameof(ProductoJson), "producto")]
public partial class editProducto : ContentPage
{
    private readonly ProductoViewModel _viewModel;
    public string ProductoJson
    {
        set
        {
            var producto = JsonSerializer.Deserialize<Producto>(value);
            _viewModel.ProductoSeleccionado = producto;
        }
    }
    public editProducto()
	{
		InitializeComponent();
        _viewModel = new ProductoViewModel();
        BindingContext = _viewModel;
	}

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Navegar a la página anterior
        await Shell.Current.GoToAsync("..");
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarCategoriasAsync();
    }
}