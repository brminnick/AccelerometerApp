using System.Diagnostics;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace AccelerometerApp
{
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
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
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
}
