using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PokeAnimation.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderPokedex : ContentView
    {
        #region Fields

        private readonly double fontBig = (double)App.Current.Resources["fontBig"];
        private readonly double fontSmall = (double)App.Current.Resources["fontSmall"];
        private readonly double headerExpandedHeight = (double)App.Current.Resources["headerExpandedHeight"];
        private readonly double headerHeight = (double)App.Current.Resources["headerHeight"];
        private readonly double screenDensityHeight = (double)App.Current.Resources["screenDensityHeight"];
        private readonly double screenDensityWidth = (double)App.Current.Resources["screenDensityWidth"];
        private double diffH = 0;

        private double lastY = 0;

        public static readonly BindableProperty ScrollYPositionProperty = BindableProperty.Create(
           nameof(ScrollYPosition),
           typeof(double),
           typeof(HeaderPokedex),
           0D);

        #endregion Fields

        #region Properties

        public double ScrollYPosition
        {
            get => (double)GetValue(ScrollYPositionProperty);
            set => SetValue(ScrollYPositionProperty, value);
        }

        #endregion Properties

        #region Constructors

        public HeaderPokedex()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        private async Task collapse(double size)
        {
            var th = this.screenDensityHeight - this.headerExpandedHeight;
            var h = th - size;
            var time = (h / th) * 300;

            await this.LayoutTo(new Rectangle(this.X, this.Y, this.Width, this.headerExpandedHeight), (uint)time, Easing.SinOut);
        }

        private async Task expand(double size)
        {
            var th = this.screenDensityHeight - this.headerExpandedHeight;
            var h = th - size;
            var time = (h / th) * 300;

            await this.LayoutTo(new Rectangle(this.X, this.Y, this.Width, this.screenDensityHeight), (uint)time, Easing.SinOut);
        }

        private async void Handle_PanUpdated(object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                //if (this.HeightRequest - diffH > headerExpandedHeight || e.TotalY - diffH > 0)
                //{
                var h = headerHeight + (e.TotalY) + diffH;
                if (h > 200)
                {
                    this.HeightRequest = headerHeight + (e.TotalY) + diffH;
                }
                //}
                diffH = 0;
                this.lastY = e.TotalY - this.lastY; ;
            }
            else if (e.StatusType == GestureStatus.Completed)
            {
                var v = (Xamarin.Forms.View)sender;
                AbsoluteLayout.SetLayoutFlags(v, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
                AbsoluteLayout.SetLayoutBounds(v, new Rectangle(0, 1, 1, -1));
                v.HeightRequest = headerHeight;
                if (this.lastY > headerExpandedHeight / 3)
                {
                    await this.expand(e.TotalY);
                }
                else
                {
                    await this.collapse(e.TotalY);
                }
            }
            else if (e.StatusType == GestureStatus.Canceled)
            {
                //this.lastY = 0;
                //var v = (View)sender;
                //AbsoluteLayout.SetLayoutFlags(v, AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);
                //AbsoluteLayout.SetLayoutBounds(v, new Rectangle(0, 1, 1, -1));
                //v.HeightRequest = headerHeight;
            }
            else if (e.StatusType == GestureStatus.Started)
            {
                var v = (Xamarin.Forms.View)sender;
                var h = v.Height;
                AbsoluteLayout.SetLayoutFlags(v, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(v, new Rectangle(0, 0, 1, 1));
                this.lastY = 0;
                ((AbsoluteLayout)v.Parent).ForceLayout();
                diffH = v.Height - h;
            }
        }

        private async void Handle_Tapped(object sender, System.EventArgs e)
        {
            if (this.Height != (double)App.Current.Resources["screenDensityHeight"])
            {
                await this.expand(0);
            }
            else
            {
                await this.collapse(0);
            }
        }

        private void ObservablePropertyBehaviorCollapse_ObservablePropertyChanged(object sender, Behaviors.ObservableBehaviorEventArgs e)
        {
            this.headerTitulo.FontSize = ((this.fontBig - this.fontSmall) * e.Proportion) + this.fontSmall;
            this.headerSubTitulo.Opacity = e.Proportion;
            this.headerButtonArea.Opacity = e.Proportion;

            this.headerButtonArea.IsVisible = e.Proportion != 0;
            this.headerSubTitulo.IsVisible = e.Proportion != 0;
        }

        private void ObservablePropertyBehaviorExpand_ObservablePropertyChanged(object sender, Behaviors.ObservableBehaviorEventArgs e)
        {
            if (e.Proportion >= 0.5)
            {
                this.headerTitulo.Opacity = 0;
                //this.headerSubTitulo.FontSize = this.fontBig;
                this.selectRegionView.Opacity = (e.Proportion - 0.5D) * 2;
                //this.headerSubTitulo.Scale = 3;
            }
            else
            {
                this.headerTitulo.Opacity = ((1 - e.Proportion) - 0.5D) * 2;
                //this.headerSubTitulo.Scale = 1 + (2 * (e.Proportion * 2));
                this.selectRegionView.Opacity = 0;
            }
            this.headerSubTitulo.TranslationY = this.headerExpandedHeight * -0.9 * e.Proportion;
            this.headerSubTitulo.Scale = 1 + (e.Proportion * 1);
            this.arrowButton.RotationX = 180 * e.Proportion;

            //this.headerTitulo.IsVisible = e.Proportion != 1;
            //this.headerSubTitulo.IsVisible = e.Proportion != 1;
            //this.selectRegionView.IsVisible = e.Proportion != 0;
        }

        #endregion Methods
    }
}