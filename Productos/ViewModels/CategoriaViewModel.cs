using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text;
using System.Windows.Input;
using Productos.Models;
using Productos.Views;
using Productos;

public class CategoriaViewModel : INotifyPropertyChanged
{
    private readonly HttpClient _httpClient;
    public ObservableCollection<Categoria> Categorias { get; set; } = new();

    public ICommand CargarCategoriasCommand { get; }
    public ICommand EliminarCategoriaCommand { get; }
    public ICommand IrAgregarCommand { get; }
    public ICommand IrEditarCommand { get; }
    public ICommand CrearCategoriaCommand { get; }
    public ICommand ActualizarCategoriaCommand { get; }

    private Categoria _categoriaSeleccionada;
    private Categoria _categoriaOriginal;

    public Categoria CategoriaSeleccionada
    {
        get => _categoriaSeleccionada;
        set
        {
            if (_categoriaSeleccionada != value)
            {
                _categoriaSeleccionada = value;
                _categoriaOriginal = new Categoria
                {
                    id = (int)(value?.id),
                    name = value?.name,
                    description = value?.description
                };
                OnPropertyChanged();
            }
        }
    }

    private Categoria _nuevaCategoria = new Categoria();
    public Categoria NuevaCategoria
    {
        get => _nuevaCategoria;
        set { _nuevaCategoria = value; OnPropertyChanged(); }
    }

    public CategoriaViewModel()
    {
        _httpClient = new HttpClient();
        CargarCategoriasCommand = new Command(async () => await CargarCategoriasAsync());
        EliminarCategoriaCommand = new Command<Categoria>(async (categoria) => await EliminarCategoriaAsync(categoria));
        IrAgregarCommand = new Command(async () => await IrACrearCategoria());
        IrEditarCommand = new Command<Categoria>(async (categoria) => await IrAEditarCategoria(categoria));
        CrearCategoriaCommand = new Command(async () => await CrearCategoriaAsync());
        ActualizarCategoriaCommand = new Command(async () => await ActualizarCategoriaAsync());

        _ = CargarCategoriasAsync();
    }

    private async Task ActualizarCategoriaAsync()
    {
        if (CategoriaSeleccionada == null || string.IsNullOrWhiteSpace(CategoriaSeleccionada.description))
        {
            await App.Current.MainPage.DisplayAlert("Error", "Por favor, complete todos los campos.", "Aceptar");
            return;
        }

        var cambios = new Dictionary<string, object>();

        // Solo incluir el nombre si ha cambiado
        if (_categoriaOriginal.name != CategoriaSeleccionada.name)
            cambios["name"] = CategoriaSeleccionada.name;

        // Solo incluir la descripción si ha cambiado
        if (_categoriaOriginal.description != CategoriaSeleccionada.description)
            cambios["description"] = CategoriaSeleccionada.description;

        if (cambios.Count == 0)
        {
            await App.Current.MainPage.DisplayAlert("Aviso", "No se realizaron cambios.", "Aceptar");
            return;
        }

        try
        {
            var categoriaJson = JsonSerializer.Serialize(cambios);
            var content = new StringContent(categoriaJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"http://localhost:3000/api/categories/{CategoriaSeleccionada.id}", content);
            response.EnsureSuccessStatusCode();

            await App.Current.MainPage.DisplayAlert("Éxito", "Categoría actualizada con éxito.", "Aceptar");
            await Shell.Current.GoToAsync("..");  // Volver a la vista anterior
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar categoría: {ex.Message}");
            await App.Current.MainPage.DisplayAlert("Error", "Hubo un problema al actualizar la categoría.", "Aceptar");
        }
    }

    private async Task CrearCategoriaAsync()
    {
        if (string.IsNullOrWhiteSpace(NuevaCategoria.name) || string.IsNullOrWhiteSpace(NuevaCategoria.description))
        {
            await App.Current.MainPage.DisplayAlert("Error", "Por favor, complete todos los campos.", "Aceptar");
            return;
        }

        try
        {
            var categoriaJson = JsonSerializer.Serialize(NuevaCategoria);
            var content = new StringContent(categoriaJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:3000/api/categories", content);
            response.EnsureSuccessStatusCode();

            NuevaCategoria = new Categoria();
            await App.Current.MainPage.DisplayAlert("Éxito", "Categoría creada con éxito.", "Aceptar");

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear categoría: {ex.Message}");
            await App.Current.MainPage.DisplayAlert("Error", "Hubo un problema al crear la categoría.", "Aceptar");
        }
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
    }

    private async Task IrAEditarCategoria(Categoria categoria)
    {
        var json = JsonSerializer.Serialize(categoria);
        await Shell.Current.GoToAsync($"{nameof(editCategoria)}?categoria={json}");
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
