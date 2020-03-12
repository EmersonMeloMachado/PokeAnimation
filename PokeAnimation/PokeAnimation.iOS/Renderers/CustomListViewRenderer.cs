using PokeAnimation.CustomControls;
using PokeAnimation.iOS.Renderers;
using UIKit;
using Xamarin.Forms;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomListView), typeof(CustomListViewRenderer))]

namespace PokeAnimation.iOS.Renderers
{
    public class CustomListViewRenderer : Xamarin.Forms.Platform.iOS.ListViewRenderer
    {
        #region Classes

        private class ListViewTableViewDelegate : UITableViewDelegate
        {
            #region Fields

            private ListView _element;
            private UITableViewSource _source;

            #endregion Fields

            #region Constructors

            public ListViewTableViewDelegate(CustomListViewRenderer renderer)
            {
                _element = renderer.Element;
                _source = renderer.Control.Source;
            }

            #endregion Constructors

            #region Methods

            private void SendScrollEvent(double y)
            {
                var element = _element as CustomListView;
                var args = new CustomListView.CancelableScrolledEventArgs(0, y);

                element?.OnScrolled(args);
            }

            public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
            {
                _source.DraggingEnded(scrollView, willDecelerate);
            }

            public override void DraggingStarted(UIScrollView scrollView)
            {
                _source.DraggingStarted(scrollView);
            }

            public override System.nfloat GetHeightForHeader(UITableView tableView, System.nint section)
            {
                return _source.GetHeightForHeader(tableView, section);
            }

            public override UIView GetViewForHeader(UITableView tableView, System.nint section)
            {
                return _source.GetViewForHeader(tableView, section);
            }

            public override void RowDeselected(UITableView tableView, Foundation.NSIndexPath indexPath)
            {
                _source.RowDeselected(tableView, indexPath);
            }

            public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
            {
                _source.RowSelected(tableView, indexPath);
            }

            public override void Scrolled(UIScrollView scrollView)
            {
                _source.Scrolled(scrollView);
                SendScrollEvent(scrollView.ContentOffset.Y);
            }

            #endregion Methods
        }

        #endregion Classes

        #region Methods

        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.Delegate = new ListViewTableViewDelegate(this);
            }
        }

        #endregion Methods
    }
}