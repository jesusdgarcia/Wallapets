using Wallapets.Model;

namespace Wallapets.Views;

public partial class DetallesPage : ContentPage
{
	public DetallesPage()
	{
		InitializeComponent();
        BindingContext = new clsDetallesVM();
    }
}