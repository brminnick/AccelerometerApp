# Accelerometer App

Let's take a look at how to implement this control in a Xamarin.Forms app.

## Walkthrough

### 1. Install NuGet Packages

1. Install the [Syncfusion.Xamarin.SfGauge NuGet Package](https://www.nuget.org/packages/Syncfusion.Xamarin.SfGauge) into each project, this includes the Xamarin.iOS, Xamarin.Android, and the .NET Standard Project (if one is being used).
2. Install the [Xamarin.Essentials NuGet Package](https://www.nuget.org/packages/Xamarin.Essentials) into each project, this includes the Xamarin.iOS, Xamarin.Android, and the .NET Standard Project (if one is being used).

### 2. Initialize SyncFusion on iOS

To utilize the SyncFusion Circular Gauge, we first need to initialize it in `AppDelegate.FinishedLaunching`:

```csharp
public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
{
    ...

    public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
    {
        ...

        Syncfusion.SfGauge.XForms.iOS.SfGaugeRenderer.Init();
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Your SyncFusion License Key");

        ...
    }
}
```

### 3. Initialize SyncFusion on Android

To utilize the SyncFusion Circular Gauge, we first need to initialize it `MainActivity.OnCreate`:

```csharp
public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
{
    ...

    protected override void OnCreate(Bundle savedInstanceState)
    {
        ...

        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Your SyncFusion License Key");

        ...
    }
}
```

### 4. Reference Mono.Android.Export

1. In the Solution Explorer, in the Android project, right-click on **References**
2. In the **References** menu, select **Edit References**

![Edit References](https://user-images.githubusercontent.com/13558917/43227940-0804184c-9015-11e8-8225-0b04b5507219.png)

3. In the **Edit References** window, select the **Packages** tab
4. In the **Packages** tab, locate `Mono.Android.Export`
5. In the **Packages** tab, ensure `Mono.Android.Export` is checked
6. In the **Edit References** window, click **OK**

![Refernce Mono.Android.Export](https://user-images.githubusercontent.com/13558917/43227949-0c77ed0e-9015-11e8-8ff5-26d9f767e095.png)

### 5. Implement SfCircularGauge in Xamarin.Forms

This app requires 3 instances of the Circular Gauge Control in our app, so let's start by creating an implementation of `SfCircularGauge`

```csharp
public class CircularGaugeView : SfCircularGauge
{
    public CircularGaugeView(string headerText, double startValue, double endValue)
    {
        Pointer = new NeedlePointer { AnimationDuration = 0.5 };

        var header = new Header
        {
            Text = headerText,
            ForegroundColor = Color.Gray
        };

        var circularGaugeScale = new Scale
        {
            Interval = (endValue - startValue) / 10,
            StartValue = startValue,
            EndValue = endValue,
            ShowTicks = true,
            ShowLabels = true,
            Pointers = { Pointer },
            MinorTicksPerInterval = 4,
        };

        Scales = new ObservableCollection<Scale> { circularGaugeScale };
        Headers = new ObservableCollection<Header> { header };
    }

    public NeedlePointer Pointer { get; }
}
```

### 6. Create Accelerometer Page

In the Xamarin.Forms project, create a new class, `AccelerometerPage.cs`:

```csharp
public class AccelerometerPage : ContentPage
{
    readonly CircularGaugeView xCircularGauge, yCircularGauge, zCircularGauge;

    public AccelerometerPage()
    {
        Icon = "Accelerometer";
        Title = "Accelerometer";

        xCircularGauge = new CircularGaugeView("X-Axis", -1, 1);
        yCircularGauge = new CircularGaugeView("Y-Axis", -1, 1);
        zCircularGauge = new CircularGaugeView("Z-Axis", -10, 10);

        var grid = new Grid
        {
            Margin = new Thickness(0, 20),
            RowDefinitions = {
                new RowDefinition { Height = new GridLength(1,GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1,GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1,GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(0.25,GridUnitType.Star) }
            },
            ColumnDefinitions = {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            }
        };
        grid.Children.Add(xCircularGauge, 0, 0);
        grid.Children.Add(yCircularGauge, 0, 1);
        grid.Children.Add(zCircularGauge, 0, 2);

        Content = grid;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        InitializeAccelerometer();
    }

    void InitializeAccelerometer()
    {
        try
        {
            Accelerometer.Start(SensorSpeed.Normal);
            Accelerometer.ReadingChanged += HandleAccelerometerReadingChanged;
        }
        catch (FeatureNotSupportedException)
        {
            Debug.WriteLine("Accelerometer Unavailable");
        }
    }

    void HandleAccelerometerReadingChanged(AccelerometerChangedEventArgs e)
    {
        Device.BeginInvokeOnMainThread(() =>
        {
            xCircularGauge.Pointer.Value = e.Reading.Acceleration.Y;
            yCircularGauge.Pointer.Value = e.Reading.Acceleration.Y;
            zCircularGauge.Pointer.Value = e.Reading.Acceleration.Y;
        });
    }
}
```

### 7. Set AccelerometerPage as the MainPage

In `App.cs`, ensure that `MainPage = new AccelerometerPage();`:

```csharp
public class App : Xamarin.Forms.Application
{
    public App()
    {
        MainPage = new AccelerometerPage();
    }
}
```

## 8. Launch the app on an iOS or Android Device

## About The Author

Brandon Minnick is a Developer Advocate at Microsoft specializing in Xamarin and Azure. As a Developer Advocate, Brandon works closely with the mobile app community, helping them create 5-star apps and provide their feedback to the Microsoft product teams to help improve our tools and empower every person and every organization on the planet to achieve more.

[@TheCodeTraveler](https://twitter.com/intent/user?user_id=3418408341)

## Resources

- [Xamarin](https://visualstudio.microsoft.com/xamarin/?WT.mc_id=none-SyncFusionBlog-bramin)
- [Xamarin.Essentials](https://docs.microsoft.com/xamarin/essentials/?WT.mc_id=none-SyncFusionBlog-bramin)
- [Azure IoT Central](https://azure.microsoft.com/services/iot-central/?WT.mc_id=none-SyncFusionBlog-bramin)
- [SyncFusion's Circular Gauge](https://www.syncfusion.com/products/xamarin/circular-gauge)
