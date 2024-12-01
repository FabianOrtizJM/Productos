using Productos.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

public class ProductoViewModel : INotifyPropertyChanged
{
    private readonly HttpClient _httpClient;
    public ObservableCollection<Producto> Productos { get; set; } = new();
    public ObservableCollection<Categoria> Categorias { get; set; } = new();

    public ICommand CargarProductosCommand { get; }
    public ICommand AgregarProductoCommand { get; }
    public ICommand EliminarProductoCommand { get; }

    private Producto _productoSeleccionado;
    public Producto ProductoSeleccionado
    {
        get => _productoSeleccionado;
        set { _productoSeleccionado = value; OnPropertyChanged(); }
    }

    public ProductoViewModel(HttpClient httpClient)
    {
        _httpClient = httpClient;

        CargarProductosCommand = new Command(async () => await CargarProductosAsync());
        AgregarProductoCommand = new Command(async () => await AgregarProductoAsync());
        EliminarProductoCommand = new Command(async () => await EliminarProductoAsync());

        _ = CargarProductosAsync();
        _ = CargarCategoriasAsync();
    }

    private async Task CargarProductosAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://api.example.com/productos");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var productos = JsonSerializer.Deserialize<List<Producto>>(json);

            Productos = new ObservableCollection<Producto>(productos);
            OnPropertyChanged(nameof(Productos));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar productos: {ex.Message}");
        }
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

    private async Task AgregarProductoAsync()
    {
        try
        {
            var nuevoProducto = new Producto
            {
                Nombre = "Nuevo Producto",
                Descripcion = "Descripción",
                Precio = 100,
                CategoriaId = Categorias.FirstOrDefault()?.Id ?? 0
            };

            var json = JsonSerializer.Serialize(nuevoProducto);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.example.com/productos", content);
            response.EnsureSuccessStatusCode();

            Productos.Add(nuevoProducto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al agregar producto: {ex.Message}");
        }
    }

    private async Task EliminarProductoAsync()
    {
        try
        {
            if (ProductoSeleccionado == null) return;

            var response = await _httpClient.DeleteAsync($"https://api.example.com/productos/{ProductoSeleccionado.Id}");
            response.EnsureSuccessStatusCode();

            Productos.Remove(ProductoSeleccionado);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar producto: {ex.Message}");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
