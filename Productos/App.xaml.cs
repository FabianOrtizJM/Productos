using Productos.Views;

namespace Productos
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Categorias), typeof(Productos.Views.Categorias));

            //MainPage = new AppShell();
            MainPage = new Login();
        }

        public void ChangeToAppShell()
        {
            MainPage = new AppShell(); // Cambia a AppShell después del login
        }
    }
}
