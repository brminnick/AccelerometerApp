using Xamarin.Forms;

namespace AccelerometerApp
{
    public class App : Application
    {
        public App()
        {
            Device.SetFlags(new[] { "Markup_Experimental" });

            MainPage = new AccelerometerPage();
        }
    }
}
