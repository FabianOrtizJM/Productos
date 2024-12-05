using Productos.ViewModels;

namespace Productos.Views;

public partial class Registro : ContentPage
{
    private readonly EmpleadoViewModel ViewModel;
    public Registro()
	{
		InitializeComponent();
		ViewModel = new EmpleadoViewModel();
		BindingContext = ViewModel;
	}
}