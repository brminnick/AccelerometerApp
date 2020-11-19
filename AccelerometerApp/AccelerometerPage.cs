using System.Linq;
using Xamarin.CommunityToolkit.Markup;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using static Xamarin.CommunityToolkit.Markup.GridRowsColumns;
namespace AccelerometerApp
{
    public class AccelerometerPage : ContentPage
    {
        public AccelerometerPage()
        {
            InitializeAccelerometer();

            Content = new Grid
            {
                Margin = new Thickness(0, 20),

                RowDefinitions = Rows.Define(
                    (Row.xGauge, Star),
                    (Row.yGauge, Star),
                    (Row.zGauge, Star)),

                ColumnDefinitions = Columns.Define(Star),

                Children =
                {
                    new CircularGaugeView("X-Axis", -1, 1).Row(Row.xGauge),
                    new CircularGaugeView("Y-Axis", -1, 1).Row(Row.yGauge),
                    new CircularGaugeView("Z-Axis", -10, 10).Row(Row.zGauge)
                }
            };

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        enum Row { xGauge, yGauge, zGauge }

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
            var grid = (Grid)Content;

            var xCircularGauge = (CircularGaugeView)grid.Children.First(x => Grid.GetRow(x) is (int)Row.xGauge);
            var yCircularGauge = (CircularGaugeView)grid.Children.First(x => Grid.GetRow(x) is (int)Row.yGauge);
            var zCircularGauge = (CircularGaugeView)grid.Children.First(x => Grid.GetRow(x) is (int)Row.zGauge);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                xCircularGauge.Pointer.Value = e.Reading.Acceleration.X;
                yCircularGauge.Pointer.Value = e.Reading.Acceleration.Y;
                zCircularGauge.Pointer.Value = e.Reading.Acceleration.Z;
            });
        }
    }
}
