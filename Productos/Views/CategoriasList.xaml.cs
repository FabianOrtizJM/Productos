using System.Collections.ObjectModel;

namespace Productos.Views;

public partial class CategoriasList : ContentPage
{
    public CategoriaViewModel ViewModel { get; set; }

    public CategoriasList()
    {
        InitializeComponent();

        ViewModel = new CategoriaViewModel();
        BindingContext = ViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Recargar las categorías cada vez que la vista aparezca
        await ViewModel.CargarCategoriasAsync();
    }
}