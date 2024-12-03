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
        EliminarCategoriaCommand = new Command(async (categoria) => await EliminarCategoriaAsync(categoria));

        _ = CargarCategoriasAsync();
    }

    private async Task CargarCategoriasAsync()
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

    private async Task EliminarCategoriaAsync(object categoriaObj)
    {
        if (categoriaObj is Categoria categoriaSeleccionada)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"http://localhost:3000/api/categories/{categoriaSeleccionada.id}");
                response.EnsureSuccessStatusCode();
                Categorias.Remove(categoriaSeleccionada);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar categoría: {ex.Message}");
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
