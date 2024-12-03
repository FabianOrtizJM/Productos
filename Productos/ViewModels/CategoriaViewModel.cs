using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Productos;
using Productos.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

public class CategoriaViewModel : INotifyPropertyChanged
{
    private readonly HttpClient _httpClient;
    public ObservableCollection<Categoria> Categorias { get; set; } = new();

    public ICommand CargarCategoriasCommand { get; }
    public ICommand EliminarCategoriaCommand { get; }
    public ICommand IrAgregarCommand { get; }

    private Categoria _categoriaSeleccionada;
    public Categoria CategoriaSeleccionada
    {
        get => _categoriaSeleccionada;
        set { _categoriaSeleccionada = value; OnPropertyChanged(); }
    }

    public CategoriaViewModel()
    {
        _httpClient = new HttpClient();
        CargarCategoriasCommand = new Command(async () => await CargarCategoriasAsync());
        EliminarCategoriaCommand = new Command<Categoria>(async (categoria) => await EliminarCategoriaAsync(categoria));
        IrAgregarCommand = new Command(async () => await IrACrearCategoria());

        _ = CargarCategoriasAsync();
    }

    public async Task CargarCategoriasAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("http://localhost:3000/api/categories");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var categorias = JsonSerializer.Deserialize<List<Categoria>>(json);

            Categorias.Clear();
            foreach (var categoria in categorias)
            {
                Categorias.Add(categoria);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar categorías: {ex.Message}");
        }
    }

    private async Task EliminarCategoriaAsync(Categoria categoria)
    {
        try
        {
            bool isConfirmed = await App.Current.MainPage.DisplayAlert(
                "Confirmación",
                "¿Estás seguro de que deseas eliminar esta categoría?",
                "Sí", "No");

            if (!isConfirmed)
                return;

            var response = await _httpClient.DeleteAsync($"http://localhost:3000/api/categories/{categoria.id}");
            response.EnsureSuccessStatusCode();

            Categorias.Remove(categoria);

            await App.Current.MainPage.DisplayAlert("Éxito", "Categoría eliminada con éxito.", "Aceptar");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar categoría: {ex.Message}");
            await App.Current.MainPage.DisplayAlert("Error", "Hubo un problema al eliminar la categoría.", "Aceptar");
        }
    }

    private async Task IrACrearCategoria()
    {
        await Shell.Current.GoToAsync(nameof(Categorias));
        //await Shell.Current.GoToAsync($"//{nameof(Categorias)}");
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
