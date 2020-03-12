using CoreAnimation;
using CoreGraphics;
using PokeAnimation.CustomControls;
using PokeAnimation.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomGradientContentView), typeof(CustomGradientContentViewRenderer))]

namespace PokeAnimation.iOS.Renderers
{
    public class CustomGradientContentViewRenderer : VisualElementRenderer<ContentView>
    {
        #region Properties

        /// <summary>
        /// Gets the underlying element typed as an <see cref="GradientContentView"/>.
        /// </summary>
        private CustomGradientContentView GradientContentView
        {
            get { return (CustomGradientContentView)Element; }
        }

        protected CAGradientLayer GradientLayer { get; set; }

        #endregion Properties

        #region Methods

        void SetOrientation()
        {
            if (GradientContentView.Orientation == GradientOrientation.Horizontal)
            {
                GradientLayer.StartPoint = new CGPoint(0, 0.5);
                GradientLayer.EndPoint = new CGPoint(1, 0.5);
            }
            else
            {
                GradientLayer.StartPoint = new CGPoint(0.5, 0);
                GradientLayer.EndPoint = new CGPoint(0.5, 1);
            }
        }

        /// <summary>
        /// Setup the gradient layer
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<ContentView> e)
        {
            base.OnElementChanged(e);

            if (GradientContentView != null &&
                NativeView != null)
            {
                // Create a gradient layer and add it to the
                // underlying UIView
                GradientLayer = new CAGradientLayer();
                GradientLayer.Frame = NativeView.Bounds;
                GradientLayer.Colors = new CGColor[]
                {
                        GradientContentView.StartColor.ToCGColor(),
                        GradientContentView.EndColor.ToCGColor()
                };

                SetOrientation();

                NativeView.Layer.InsertSublayer(GradientLayer, 0);
            }
        }

        /// <summary>
        /// Update the underlying controls as needed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (GradientLayer != null && GradientContentView != null)
            {
                // Turn off Animations
                CATransaction.Begin();
                CATransaction.DisableActions = true;

                if (e.PropertyName == CustomGradientContentView.StartColorProperty.PropertyName)
                    GradientLayer.Colors[0] = GradientContentView.StartColor.ToCGColor();

                if (e.PropertyName == CustomGradientContentView.EndColorProperty.PropertyName)
                    GradientLayer.Colors[1] = GradientContentView.EndColor.ToCGColor();

                if (e.PropertyName == VisualElement.WidthProperty.PropertyName ||
                    e.PropertyName == VisualElement.HeightProperty.PropertyName)
                    GradientLayer.Frame = NativeView.Bounds;

                if (e.PropertyName == CustomGradientContentView.OrientationProperty.PropertyName)
                    SetOrientation();

                CATransaction.Commit();
            }
        }

        #endregion Methods
    }
}