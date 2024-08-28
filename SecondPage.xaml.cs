namespace SfMAUI;

public partial class SecondPage : ContentPage
{
	public SecondPage()
	{
		InitializeComponent();
        Dispatcher.StartTimer(TimeSpan.FromSeconds(0.7), () =>
        {
            Dispatcher.Dispatch(UpdateLabels);
            return true;
        });
    }

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        if (App.Current?.MainPage != null)
            App.Current.MainPage.Navigation.PushAsync(new MainPage());
    }

    private void UpdateLabels()
    {
        double heapSize = GC.GetTotalMemory(false)
                          / 1024d;

        HeapSpan.Text = $"{heapSize:N2} KB";
        RamSpan.Text = $"{HardwareInfo.GetMemoryUsageCurrentProcess():N2} MB";
    }

    private void CollectGarbage(object sender, EventArgs e)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }
}
