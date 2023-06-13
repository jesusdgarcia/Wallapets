using Wallapets.Views;

namespace Wallapets;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute("detallespage", typeof(DetallesPage));
        Routing.RegisterRoute("registerpage", typeof(RegisterPage));
        Routing.RegisterRoute("mensajespage", typeof (MensajesPage));
        Routing.RegisterRoute("editpage", typeof(EditPage));
        var getUserSavedKey = Preferences.Get("UserAlreadyLoggedIn", false);
        if (getUserSavedKey == true)
        {
            ShellView.CurrentItem = DashboardPage;
        }
        else
        {
            ShellView.CurrentItem = LoginPage;
        }
    }
}
