using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Productos.ViewModels;
using System.Net.Http;

namespace Productos
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            // Configurar servicios de HttpClient y ViewModels
            builder.Services.AddSingleton<HttpClient>(); // Agregar HttpClient
            builder.Services.AddSingleton<CategoriaViewModel>(); // Agregar ViewModel para Categoría
            builder.Services.AddSingleton<ProductoViewModel>(); // Agregar ViewModel para Producto
            // Agregar la página principal y las páginas necesarias
            builder.Services.AddSingleton<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
