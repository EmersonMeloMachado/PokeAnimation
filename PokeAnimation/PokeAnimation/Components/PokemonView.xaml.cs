using PokeAnimation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PokeAnimation.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PokemonView : ContentView
    {
        #region Fields

        private Action _onClosing;
        private Rectangle _recInit;
        private double buttonSize = (double)App.Current.Resources["buttonSize"];
        private double density = (double)App.Current.Resources["screenDensity"];
        private double screenHeight = (double)App.Current.Resources["screenDensityHeight"];
        private double screenWidth = (double)App.Current.Resources["screenDensityWidth"];

        #endregion Fields

        #region Properties

        public ObservableCollection<KeyValuePair<Color, string>> Itens { get; set; } = new ObservableCollection<KeyValuePair<Color, string>>();

        #endregion Properties

        #region Constructors

        public PokemonView()
        {
            this.Itens.Add(new KeyValuePair<Color, string>(Color.Red, "Red"));
            this.Itens.Add(new KeyValuePair<Color, string>(Color.Blue, "Blue"));
            this.Itens.Add(new KeyValuePair<Color, string>(Color.Green, "Green"));
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        private void ObservablePropertyBehavior_ObservablePropertyChanged(object sender, Behaviors.ObservableBehaviorEventArgs e)
        {
        }

        private async void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            var view = (Xamarin.Forms.View)sender;
            var recInit = this._recInit;
            await view.ScaleTo(0.85, 100);
            await view.ScaleTo(1, 100);

            this.expanded.IsVisible = false;
            this.colapsed.IsVisible = true;
            this._onClosing?.Invoke();
            this.fechaMonsterView.FadeTo(0, 100, Easing.SinOut);
            this.expandedId.FadeTo(0, 100, Easing.SinOut);
            this.expandedName.FadeTo(0, 100, Easing.SinOut);
            await this.fechaMonsterView.FadeTo(0, 100, Easing.SinOut);
            var an = new Animation(
                v =>
                {
                    AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.WidthProportional | AbsoluteLayoutFlags.XProportional);
                    AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0, recInit.Y - (recInit.Y * v), 1, recInit.Height + ((screenHeight - recInit.Height) * v)));
                    this.fechaMonsterView.HeightRequest = v * 40;
                    this.image.Scale = 1 + (4 * v);
                    this.image.TranslationX = (180 - 40) * v;
                    this.image.TranslationY = (150 - 30) * v;
                }, 1, 0, Easing.SinOut);
            an.Commit(this, "teste", length: 300, finished: async (d, b) =>
            {
                this.colapsedId.FadeTo(1, 100, Easing.SinOut);
                await this.colapsedName.FadeTo(1, 100, Easing.SinOut);
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.TranslationY = 5000;
                });
            });
        }

        public async void Open(MonsterResume monster, Rectangle recInit, Action onClosing = null)
        {
            this._onClosing = onClosing;
            this.expanded.IsVisible = false;
            this.BindingContext = monster;
            this._recInit = recInit;
            AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.WidthProportional | AbsoluteLayoutFlags.XProportional);
            AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0, recInit.Y, 1, recInit.Height));
            this.TranslationY = 0;
            this.colapsedId.FadeTo(0, 100, Easing.SinOut);
            await this.colapsedName.FadeTo(0, 100, Easing.SinOut);
            var an = new Animation(
                v =>
                {
                    AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.WidthProportional | AbsoluteLayoutFlags.XProportional);
                    AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0, recInit.Y - (recInit.Y * v), 1, recInit.Height + ((screenHeight - recInit.Height) * v)));
                    this.fechaMonsterView.HeightRequest = v * 40;
                    this.image.Scale = 1 + (4 * v);
                    this.image.TranslationX = (180 - 40) * v;
                    this.image.TranslationY = (150 - 30) * v;
                }, 0, 1, Easing.SinOut);
            an.Commit(this, "teste", length: 300, finished: async (d, b) =>
            {
                this.colapsed.IsVisible = false;
                this.expanded.IsVisible = true;
                this.fechaMonsterView.FadeTo(1, 100, Easing.SinOut);
                this.expandedId.FadeTo(1, 100, Easing.SinOut);
                this.expandedName.FadeTo(1, 100, Easing.SinOut);
            });
        }

        #endregion Methods
    }
}