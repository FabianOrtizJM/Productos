using System.Collections.ObjectModel;

namespace Productos.Views;

public partial class ProductosList : ContentPage
{
    public ProductoViewModel ViewModel { get; set; }
    public ProductosList()
    {
        InitializeComponent();

        ViewModel = new ProductoViewModel();
        BindingContext = ViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.CargarProductosAsync();
    }
}