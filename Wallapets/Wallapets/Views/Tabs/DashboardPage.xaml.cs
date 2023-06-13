using Wallapets.Model;

namespace Wallapets.Views.Tabs;

public partial class DashboardPage : ContentPage
{
	public DashboardPage()
	{
		InitializeComponent();
    }

    /// <summary>
    /// Metodo que cuando inicia la pagina
    /// muestren las mascotas subidas excluyendo a las del usuario 
    /// y se cargen las listas de especies y localidades
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        var binding = (clsDashboardVM)this.BindingContext;
        binding.CargarMascotas();
        binding.CargarLista();
    }
}