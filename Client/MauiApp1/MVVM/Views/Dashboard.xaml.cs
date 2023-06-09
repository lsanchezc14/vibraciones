using OPCClientTest;

namespace MauiApp1.MVVM.Views;

public partial class Dashboard : ContentPage
{
	private bool response;
	public Dashboard()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		var opcConnection = new Connect();

		var response = await opcConnection.connectPrint();

		boolAnomalia.IsChecked = response;
    }

}