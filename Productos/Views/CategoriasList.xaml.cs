using System.Collections.ObjectModel;

namespace Productos.Views;

public partial class CategoriasList : ContentPage
{
    public ObservableCollection<Categoria> Categorias { get; set; }

    public CategoriasList()
    {
        InitializeComponent();

        // Lista de categorías con los campos Nombre y Descripción
        Categorias = new ObservableCollection<Categoria>
        {
            new Categoria { Nombre = "Electrónica", Descripcion = "Categoría de productos electrónicos" },
            new Categoria { Nombre = "Muebles", Descripcion = "Categoría de productos para el hogar" },
            new Categoria { Nombre = "Ropa", Descripcion = "Categoría de productos de vestimenta" }
        };

        BindingContext = this;
    }
}

public class Categoria
{
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
}