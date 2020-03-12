using Ninject;
using PokeAnimation.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PokeAnimation.MarkupExtension
{
    [ContentProperty("TypeInstance")]
    public class IOCResource : IMarkupExtension<IBaseModelView>
    {
        #region Properties

        public Type TypeInstance
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public IBaseModelView ProvideValue(IServiceProvider serviceProvider)
        {
            var r = (IBaseModelView)App.Container.Get(this.TypeInstance);
            return r;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return (this as IMarkupExtension<IBaseModelView>).ProvideValue(serviceProvider);
        }

        #endregion Methods
    }
}