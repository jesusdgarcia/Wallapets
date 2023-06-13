using Wallapets.Model;

namespace Wallapets.Views.Tabs;

public partial class CreatePage : ContentPage
{
	public CreatePage()
	{
		InitializeComponent();
		BindingContext = new clsCreateVM();
    }
}