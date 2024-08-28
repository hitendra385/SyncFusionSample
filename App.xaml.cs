namespace SfMAUI;

public partial class App : Application
{
	public static List<WeakReference> WeakReferencesObject = new List<WeakReference>();
	public App()
	{
		InitializeComponent();
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("");//add license key
        MainPage = new NavigationPage(new MainPage());
	}
}

