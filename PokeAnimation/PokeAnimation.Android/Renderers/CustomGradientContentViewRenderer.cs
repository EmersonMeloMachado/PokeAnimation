using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using PokeAnimation.CustomControls;
using PokeAnimation.Droid.Renderers;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomGradientContentView), typeof(CustomGradientContentViewRenderer))]

namespace PokeAnimation.Droid.Renderers
{
    public class CustomGradientContentViewRenderer : ViewRenderer<CustomGradientContentView, Android.Views.View>
    {
        #region Properties

        /// <summary>
        /// Gets the underlying element typed as an <see cref="GradientContentView"/>.
        /// </summary>
        private CustomGradientContentView GradientContentView
        {
            get { return (CustomGradientContentView)Element; }
        }

        public GradientDrawable GradientDrawable { get; set; }

        #endregion Properties

        #region Constructors

        public CustomGradientContentViewRenderer(Context context) : base(context)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Setup the gradient layer
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<CustomGradientContentView> e)
        {
            base.OnElementChanged(e);

            if (GradientContentView != null)
            {
                //ShapeDrawable sd = new ShapeDrawable(new RectShape());
                GradientDrawable = new GradientDrawable();
                GradientDrawable.SetColors(new int[] { GradientContentView.StartColor.ToAndroid(), GradientContentView.EndColor.ToAndroid() });
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (GradientDrawable != null && GradientContentView != null)
            {
                if (e.PropertyName == CustomGradientContentView.StartColorProperty.PropertyName ||
                    e.PropertyName == CustomGradientContentView.EndColorProperty.PropertyName)
                {
                    GradientDrawable.SetColors(new int[] { GradientContentView.StartColor.ToAndroid(), GradientContentView.EndColor.ToAndroid() });
                    Invalidate();
                }

                if (e.PropertyName == Xamarin.Forms.VisualElement.WidthProperty.PropertyName ||
                    e.PropertyName == Xamarin.Forms.VisualElement.HeightProperty.PropertyName ||
                    e.PropertyName == CustomGradientContentView.OrientationProperty.PropertyName)
                {
                    Invalidate();
                }
            }
        }

        public override void Draw(Canvas canvas)
        {
            GradientDrawable.Bounds = canvas.ClipBounds;
            GradientDrawable.SetOrientation(GradientContentView.Orientation == GradientOrientation.Vertical ? GradientDrawable.Orientation.TopBottom
                : GradientDrawable.Orientation.LeftRight);
            GradientDrawable.Draw(canvas);
        }

        #endregion Methods
    }
}