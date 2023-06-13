using Wallapets.Model;

namespace Wallapets.Views;

public partial class EditPage : ContentPage
{
	public EditPage()
	{
		InitializeComponent();
        BindingContext = new clsEditVM();
    }
}