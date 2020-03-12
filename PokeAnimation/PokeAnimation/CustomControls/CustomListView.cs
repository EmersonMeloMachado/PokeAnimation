using System;
using Xamarin.Forms;

namespace PokeAnimation.CustomControls
{
    public class CustomListView : Xamarin.Forms.ListView
    {
        #region Classes

        public class CancelableScrolledEventArgs : ScrolledEventArgs
        {
            #region Properties

            public bool Cancel { get; set; }

            #endregion Properties

            #region Constructors

            public CancelableScrolledEventArgs(double x, double y) : base(x, y)
            {
            }

            #endregion Constructors
        }

        #endregion Classes

        #region Fields

        private double _scrollYPosition;

        #endregion Fields

        #region Properties

        public double ScrollYPosition
        {
            get => this._scrollYPosition;
            set
            {
                this._scrollYPosition = value;
                OnPropertyChanged(nameof(ScrollYPosition));
            }
        }

        #endregion Properties

        #region Constructors

        public CustomListView() : base(ListViewCachingStrategy.RetainElement)
        {
        }

        public CustomListView(ListViewCachingStrategy strategy) : base(strategy)
        {
        }

        #endregion Constructors

        #region Events

        public event EventHandler<CancelableScrolledEventArgs> Scrolled;

        #endregion Events

        #region Methods

        public void OnScrolled(CancelableScrolledEventArgs args)
        {
            this.ScrollYPosition = args.ScrollY;
            Scrolled?.Invoke(this, args);
        }

        #endregion Methods
    }
}