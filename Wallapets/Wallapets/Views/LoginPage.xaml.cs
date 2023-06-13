using Wallapets.Model;

namespace Wallapets.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        BindingContext = new clsLoginVM();
    }
}