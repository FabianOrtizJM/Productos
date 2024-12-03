using System.Text.Json;
using Productos.Models;

namespace Productos.Views;
[QueryProperty(nameof(CategoriaJson), "categoria")]
public partial class editCategoria : ContentPage
{
    private readonly CategoriaViewModel _viewModel;
    public string CategoriaJson
    {
        set
        {
            var categoria = JsonSerializer.Deserialize<Categoria>(value);
            _viewModel.CategoriaSeleccionada = categoria;
        }
    }
    public editCategoria()
	{
		InitializeComponent();
        _viewModel = new CategoriaViewModel();
        BindingContext = _viewModel;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Navegar a la página anterior
        await Shell.Current.GoToAsync("..");
    }
}