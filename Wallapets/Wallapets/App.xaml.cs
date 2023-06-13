using Wallapets.Views;

namespace Wallapets;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        
        MainPage = new AppShell();
	}
}
