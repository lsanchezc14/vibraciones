using MauiApp1.MVVM.Views;

namespace MauiApp1;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new Dashboard();
	}
}
