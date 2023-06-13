using Wallapets.Model;

namespace Wallapets.Views.Tabs;

public partial class ChatPage : ContentPage
{
	public ChatPage()
	{
		InitializeComponent();
    }

    /// <summary>
    /// Metodo que cuando se inicia la pagina
    /// se carge el listado de chats
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        var binding = (clsChatVM)this.BindingContext;
        binding.CargarChats();
    }
}