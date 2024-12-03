using System.Collections.ObjectModel;

namespace Productos.Views;

public partial class ProductosList : ContentPage
{
    public ObservableCollection<Producto> Productos { get; set; }

    public ProductosList()
    {
        InitializeComponent();

        // Lista de productos con los nuevos campos Precio y Categoria
        Productos = new ObservableCollection<Producto>
            {
                new Producto { Nombre = "Producto 1", Descripcion = "Descripción del producto 1", Precio = 100.00, Categoria = "Electrónica" },
                new Producto { Nombre = "Producto 2", Descripcion = "Descripción del producto 2", Precio = 200.50, Categoria = "Muebles" },
                new Producto { Nombre = "Producto 3", Descripcion = "Descripción del producto 3", Precio = 300.99, Categoria = "Ropa" }
            };

        BindingContext = this;
    }
}

// Clase del modelo de datos
public class Producto
{
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public double Precio { get; set; } // Nuevo campo para Precio
    public string Categoria { get; set; } // Nuevo campo para Categoria
}