using System.Collections.ObjectModel;

namespace Productos.Views;

public partial class CategoriasList : ContentPage
{
    public ObservableCollection<Categoria> Categorias { get; set; }

    public CategoriasList()
    {
        InitializeComponent();

        // Lista de categor�as con los campos Nombre y Descripci�n
        Categorias = new ObservableCollection<Categoria>
        {
            new Categoria { Nombre = "Electr�nica", Descripcion = "Categor�a de productos electr�nicos" },
            new Categoria { Nombre = "Muebles", Descripcion = "Categor�a de productos para el hogar" },
            new Categoria { Nombre = "Ropa", Descripcion = "Categor�a de productos de vestimenta" }
        };

        BindingContext = this;
    }
}

public class Categoria
{
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
}