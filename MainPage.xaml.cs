using Syncfusion.Maui.Scheduler;

namespace SfMAUI;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
        App.WeakReferencesObject.Add(new WeakReference(calendarView));
    }
}
