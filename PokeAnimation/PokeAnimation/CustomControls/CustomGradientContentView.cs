using Xamarin.Forms;

namespace PokeAnimation.CustomControls
{
    public class CustomGradientContentView : ContentView
    {
        #region Fields

        public static readonly BindableProperty EndColorProperty =
            BindableProperty.Create<CustomGradientContentView, Color>(x => x.EndColor, Color.Black, BindingMode.OneWay);

        public static readonly BindableProperty OrientationProperty =
            BindableProperty.Create<CustomGradientContentView, GradientOrientation>(x => x.Orientation, GradientOrientation.Vertical, BindingMode.OneWay);

        public static readonly BindableProperty StartColorProperty =
            BindableProperty.Create<CustomGradientContentView, Color>(x => x.StartColor, Color.White, BindingMode.OneWay);

        #endregion Fields

        #region Properties

        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }

        public GradientOrientation Orientation
        {
            get { return (GradientOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }

        #endregion Properties
    }

    public enum GradientOrientation
    {
        Vertical,

        Horizontal
    }
}