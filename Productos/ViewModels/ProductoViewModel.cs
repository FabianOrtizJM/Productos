using Productos.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text;
using System.Windows.Input;
using System.Net.Http;
using Productos;
using System.Text.Json.Serialization;
using System.Globalization;
using Productos.Views;

public class ProductoViewModel : INotifyPropertyChanged
{
    private readonly HttpClient _httpClient;
    public ObservableCollection<Producto> Productos { get; set; } = new();
    public ObservableCollection<Categoria> Categorias { get; set; } = new();

    public ICommand CargarProductosCommand { get; }
    public ICommand EliminarProductoCommand { get; }
    public ICommand IrAgregarCommand { get; }
    public ICommand IrEditarCommand { get; }
    public ICommand CrearProductoCommand { get; }
    public ICommand EditarProductoCommand { get; }

    private Producto _productoSeleccionado;
    private Producto _productoOriginal;

    public Producto ProductoSeleccionado
    {
        get => _productoSeleccionado;
        set
        {
            if (_productoSeleccionado != value)
            {
                _productoSeleccionado = value;
                _productoOriginal = new Producto
                {
                    id = value?.id ?? 0,
                    name = value?.name,
                    description = value?.description,
                    price = value?.price ?? "0",
                    categoryId = value?.categoryId ?? 0,
                    Category = value?.Category
                };
                OnPropertyChanged();
            }
        }
    }

    private Producto _nuevoProducto = new Producto();
    public Producto NuevoProducto
    {
        get => _nuevoProducto;
        set { _nuevoProducto = value; OnPropertyChanged(); }
    }

    public ProductoViewModel()
    {
        _httpClient = new HttpClient();

        CargarProductosCommand = new Command(async () => await CargarProductosAsync());
        EliminarProductoCommand = new Command<Producto>(async (producto) => await EliminarProductoAsync(producto));
        IrAgregarCommand = new Command(async () => await IrACrearProducto());
        IrEditarCommand = new Command<Producto>(async (producto) => await IrAEditarProducto(producto));
        CrearProductoCommand = new Command(async () => await CrearProductoAsync());
        EditarProductoCommand = new Command(async () => await EditarProductoAsync());

        _ = CargarProductosAsync();
    }

    public async Task CargarProductosAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("http://localhost:3000/api/products");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            var jsonObject = JsonSerializer.Deserialize<JsonElement>(json);

            // Acceder al campo "data" del JSON
            var productosJson = jsonObject.GetProperty("data").ToString();

            // Deserializar los productos desde el campo "data"
            var productos = JsonSerializer.Deserialize<List<Producto>>(productosJson);

            Productos.Clear();
            foreach (var producto in productos)
            {
                Productos.Add(producto);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar productos: {ex.Message}");
            await App.Current.MainPage.DisplayAlert("Error", $"Error al cargar productos: {ex.Message}", "Aceptar");
        }
    }

    private async Task CrearProductoAsync()
    {
        if (string.IsNullOrWhiteSpace(NuevoProducto.name) || string.IsNullOrWhiteSpace(NuevoProducto.description) || string.IsNullOrWhiteSpace(NuevoProducto.price) || NuevoProducto.Category == null)
        {
            await App.Current.MainPage.DisplayAlert("Error", "Por favor, complete todos los campos.", "Aceptar");
            return;
        }

        if (!decimal.TryParse(NuevoProducto.price, out decimal precioDecimal) || precioDecimal <= 0)
        {
            await App.Current.MainPage.DisplayAlert("Error", "El precio debe ser un número válido mayor a 0.", "Aceptar");
            return;
        }

        try
        {
            // Setear stock en 0 y obtener categoryId
            var productoParaEnviar = new
            {
                name = NuevoProducto.name,
                price = precioDecimal,
                stock = 0,
                categoryId = NuevoProducto.Category.id,
                description = NuevoProducto.description
            };

            var productoJson = JsonSerializer.Serialize(productoParaEnviar);
            var content = new StringContent(productoJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:3000/api/products", content);
            response.EnsureSuccessStatusCode();

            Productos.Add(NuevoProducto);
            NuevoProducto = new Producto(); // Limpiar el formulario
            await App.Current.MainPage.DisplayAlert("Éxito", "Producto creado con éxito.", "Aceptar");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear producto: {ex.Message}");
            await App.Current.MainPage.DisplayAlert("Error", "Hubo un problema al crear el producto.", "Aceptar");
        }
    }

    private async Task EditarProductoAsync()
    {
        if (string.IsNullOrWhiteSpace(ProductoSeleccionado.name) || string.IsNullOrWhiteSpace(ProductoSeleccionado.description) || string.IsNullOrWhiteSpace(ProductoSeleccionado.price) || ProductoSeleccionado.Category == null)
        {
            await App.Current.MainPage.DisplayAlert("Error", "Por favor, complete todos los campos.", "Aceptar");
            return;
        }

        if (!decimal.TryParse(ProductoSeleccionado.price, out decimal precioDecimal) || precioDecimal <= 0)
        {
            await App.Current.MainPage.DisplayAlert("Error", "El precio debe ser un número válido mayor a 0.", "Aceptar");
            return;
        }

        try
        {
            var productoParaEnviar = new
            {
                name = ProductoSeleccionado.name,
                price = precioDecimal,
                stock = 0,
                categoryId = ProductoSeleccionado.Category.id,
                description = ProductoSeleccionado.description
            };

            var productoJson = JsonSerializer.Serialize(productoParaEnviar);
            var content = new StringContent(productoJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"http://localhost:3000/api/products/{ProductoSeleccionado.id}", content);
            response.EnsureSuccessStatusCode();

            await App.Current.MainPage.DisplayAlert("Éxito", "Producto editado con éxito.", "Aceptar");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al editar producto: {ex.Message}");
            await App.Current.MainPage.DisplayAlert("Error", "Hubo un problema al editar el producto.", "Aceptar");
        }
    }

    private async Task EliminarProductoAsync(Producto producto)
    {
        try
        {
            bool isConfirmed = await App.Current.MainPage.DisplayAlert(
                "Confirmación",
                "¿Estás seguro de que deseas eliminar este producto?",
                "Sí", "No");

            if (!isConfirmed) return;

            var response = await _httpClient.DeleteAsync($"http://localhost:3000/api/products/{producto.id}");
            response.EnsureSuccessStatusCode();
            Productos.Remove(producto);

            await App.Current.MainPage.DisplayAlert("Éxito", "Producto eliminado con éxito.", "Aceptar");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar producto: {ex.Message}");
            await App.Current.MainPage.DisplayAlert("Error", "Hubo un problema al eliminar el producto.", "Aceptar");
        }
    }

    private async Task IrACrearProducto()
    {
        await Shell.Current.GoToAsync(nameof(ProductosV)); // Ajusta según tu ruta
    }

    private async Task IrAEditarProducto(Producto producto)
    {
        var json = JsonSerializer.Serialize(producto);
        await Shell.Current.GoToAsync($"{nameof(editProducto)}?producto={json}");
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
            // Asignar categoría por defecto después de cargar
            if (Categorias.Any())
            {
                NuevoProducto.Category = Categorias.First();
                OnPropertyChanged(nameof(NuevoProducto));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar categorías: {ex.Message}");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
