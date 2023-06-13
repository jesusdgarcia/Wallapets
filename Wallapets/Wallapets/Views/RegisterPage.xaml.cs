using Wallapets.Model;

namespace Wallapets.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
        BindingContext = new clsRegisterVM();
    }
}