using Syncfusion.Maui.Scheduler;

namespace SfMAUI;

public partial class CalendarPage : ContentPage
{
	public CalendarPage()
	{
		InitializeComponent();
        App.WeakReferencesObject.Add(new WeakReference(calendarView));
    }
}
