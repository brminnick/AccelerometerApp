using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace AccelerometerApp
{
    public class AccelerometerPage : ContentPage
    {
        readonly CircularGaugeView _xCircularGauge, _yCircularGauge, _zCircularGauge;

        public AccelerometerPage()
        {
            InitializeAccelerometer();

            _xCircularGauge = new CircularGaugeView("X-Axis", -1, 1);
            _yCircularGauge = new CircularGaugeView("Y-Axis", -1, 1);
            _zCircularGauge = new CircularGaugeView("Z-Axis", -10, 10);

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
            grid.Children.Add(_xCircularGauge, 0, 0);
            grid.Children.Add(_yCircularGauge, 0, 1);
            grid.Children.Add(_zCircularGauge, 0, 2);

            Content = grid;

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        void InitializeAccelerometer()
        {
            try
            {
                Accelerometer.Start(SensorSpeed.Default);
                Accelerometer.ReadingChanged += HandleAccelerometerReadingChanged;
            }
            catch (FeatureNotSupportedException)
            {
                System.Diagnostics.Debug.WriteLine("Accelerometer Unavailable");
            }
        }

        void HandleAccelerometerReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                _xCircularGauge.Pointer.Value = e.Reading.Acceleration.X;
                _yCircularGauge.Pointer.Value = e.Reading.Acceleration.Y;
                _zCircularGauge.Pointer.Value = e.Reading.Acceleration.Z;
            });
        }
    }
}
