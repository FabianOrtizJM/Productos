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
    public ICommand AgregarCategoriaCommand { get; }
    public ICommand EliminarCategoriaCommand { get; }

    private Categoria _categoriaSeleccionada;
    public Categoria CategoriaSeleccionada
    {
        get => _categoriaSeleccionada;
        set { _categoriaSeleccionada = value; OnPropertyChanged(); }
    }

    public CategoriaViewModel(HttpClient httpClient)
    {
        _httpClient = httpClient;

        CargarCategoriasCommand = new Command(async () => await CargarCategoriasAsync());
        AgregarCategoriaCommand = new Command(async () => await AgregarCategoriaAsync());
        EliminarCategoriaCommand = new Command(async () => await EliminarCategoriaAsync());

        _ = CargarCategoriasAsync();
    }

    private async Task CargarCategoriasAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://api.example.com/categorias");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var categorias = JsonSerializer.Deserialize<List<Categoria>>(json);

            Categorias = new ObservableCollection<Categoria>(categorias);
            OnPropertyChanged(nameof(Categorias));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar categorías: {ex.Message}");
        }
    }

    private async Task AgregarCategoriaAsync()
    {
        try
        {
            var nuevaCategoria = new Categoria { Nombre = "Nueva Categoría" };
            var json = JsonSerializer.Serialize(nuevaCategoria);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.example.com/categorias", content);
            response.EnsureSuccessStatusCode();

            Categorias.Add(nuevaCategoria);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al agregar categoría: {ex.Message}");
        }
    }

    private async Task EliminarCategoriaAsync()
    {
        try
        {
            if (CategoriaSeleccionada == null) return;

            var response = await _httpClient.DeleteAsync($"https://api.example.com/categorias/{CategoriaSeleccionada.Id}");
            response.EnsureSuccessStatusCode();

            Categorias.Remove(CategoriaSeleccionada);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar categoría: {ex.Message}");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
