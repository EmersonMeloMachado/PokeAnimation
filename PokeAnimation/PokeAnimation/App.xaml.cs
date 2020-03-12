using Ninject;
using PokeAnimation.Sevices;
using PokeAnimation.Sevices.Interface;
using PokeAnimation.View;
using PokeAnimation.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PokeAnimation
{
    internal class IOC : Ninject.Modules.NinjectModule
    {
        #region Methods

        public override void Load()
        {
            this.Bind<IPokeApi>().To<PokeApi>();
            this.Bind<PokemonViewModel>().To<PokemonViewModel>();
            this.Bind<PokedexViewModel>().To<PokedexViewModel>();
            this.Bind<MontersListAllResumeViewModel>().To<MontersListAllResumeViewModel>();
        }

        #endregion Methods
    }

    public partial class App : Application
    {
        #region Properties

        public static IKernel Container { get; set; }

        public new static App Current
        {
            get
            {
                return (App)Application.Current;
            }
        }

        public PokemonViewModel PokemonViewModel { get; private set; } = new PokemonViewModel();

        #endregion Properties

        #region Constructors

        public App()
        {
            InitializeComponent();

            this.Resources["screenDensity"] = DeviceDisplay.MainDisplayInfo.Density;
            this.Resources["screenDensityHeight"] = (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) - new OnPlatform<double> { Android = 20 };
            this.Resources["screenDensityWidth"] = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            this.Resources["screenDensityWidthMinusButtonSize"] = (double)this.Resources["screenDensityWidth"] - (double)this.Resources["buttonSize"];

            App.Container = new Ninject.StandardKernel(new IOC());

            MainPage = new PokedexPage();
        }

        #endregion Constructors

        #region Methods

        protected override void OnResume()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnStart()
        {
        }

        public T GetResource<T>(string name)
        {
            return (T)this.Resources[name];
        }

        #endregion Methods
    }
}