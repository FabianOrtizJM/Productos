namespace Productos
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Navigated += OnShellNavigated;
        }

        private async void OnShellNavigated(object sender, ShellNavigatedEventArgs e)
        {
            if (e.Current?.Location.OriginalString.Contains("AccionTab") == true)
            {
                await EjecutarAccion();
            }
        }

        private async Task EjecutarAccion()
        {
            await DisplayAlert("Acción", "Sesión cerrada correctamente", "OK");
            ((App)Application.Current).ChangeToAppLogin();
        }
    }
}
