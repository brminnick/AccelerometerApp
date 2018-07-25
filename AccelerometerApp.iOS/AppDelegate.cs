using Foundation;
using UIKit;

namespace AccelerometerApp.iOS
{
    [Register(nameof(AppDelegate))]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            #error Syncfusion License Key Missing. Retrive your key here: https://www.syncfusion.com/account/downloads
            Syncfusion.SfGauge.XForms.iOS.SfGaugeRenderer.Init();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Your SyncFusion License Key");

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }
}
