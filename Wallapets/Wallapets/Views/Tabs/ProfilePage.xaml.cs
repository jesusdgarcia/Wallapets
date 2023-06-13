using Wallapets.Model;

namespace Wallapets.Views.Tabs;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
    }

    /// <summary>
    /// Metodo que cuando inicia la pagina
    /// muestren las mascotas subidas del usuario 
    /// y se cargen dicho usuario
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        var binding = (clsProfileVM)this.BindingContext;
        binding.cargarMascotas();
        binding.cargarUsuario();
    }
}