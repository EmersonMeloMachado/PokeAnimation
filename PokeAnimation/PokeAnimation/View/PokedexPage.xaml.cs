using PokeAnimation.CustomControls;
using PokeAnimation.Model;
using PokeAnimation.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PokeAnimation.View
{
    [DesignTimeVisible(false)]
    public partial class PokedexPage : ContentPage
    {
        #region Fields

        private double position;
        private MontersListAllResumeViewModel vw;

        #endregion Fields

        #region Constructors

        public PokedexPage()
        {
            InitializeComponent();

            vw = (MontersListAllResumeViewModel)this.BindingContext;
        }

        #endregion Constructors

        #region Methods

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            vw.LoadCommand.Execute(null);
        }

        private void HeaderPokedex_ObservablePropertyChanged(object sender, Behaviors.ObservableBehaviorEventArgs e)
        {
            if(e.Proportion == 0)
            {
                this.monsterResumeListView.Scale = 1;
                this.monsterResumeListView.TranslationY = 0;
                this.shadowListView.Opacity = 0;
                this.shadowListView.TranslationY = 5000;
            }
            else
            {
                var p = ((e.Proportion * 2) > 1 ? 1 : (e.Proportion * 2));
                this.monsterResumeListView.Scale = 1 - (0.05 * p);
                this.monsterResumeListView.TranslationY = this.monsterResumeListView.Height * 0.05 * p;
                this.shadowListView.Opacity = 0.8 * p;
                this.shadowListView.TranslationY = 0;
            }
        }

        private void monsterResumeListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listView = (CustomListView)sender;
            vw.Pokemon = (MonsterResume)e.SelectedItem;
            listView.SelectedItem = null;
        }

        private async void TapGestureRecognizer(object sender, EventArgs e)
        {
            var view = (Xamarin.Forms.View)sender;
            var monsterResume = (MonsterResume)view.BindingContext;

            await view.ScaleTo(0.85, 100);
            await view.ScaleTo(1, 100);
            sombra.Opacity = 0;
            sombra.IsVisible = true;
            await sombra.FadeTo(0.8, 250, Easing.SinOut);
            var scrollPosition = (this.monsterResumeListView.ScrollYPosition);
            var index = ((IEnumerable<MonsterResume>)this.monsterResumeListView.ItemsSource).ToList().IndexOf(monsterResume);
            position = ((index * this.monsterResumeListView.RowHeight) - scrollPosition) + 200;

            monsterView.Open(
                monsterResume,
                new Rectangle(0, position, 1, this.monsterResumeListView.RowHeight),
                onClosing: async () =>
                {
                    await Task.Delay(250);
                    await this.sombra.FadeTo(0, 250, Easing.SinOut);
                    this.sombra.IsVisible = false;
                });
        }

        #endregion Methods
    }
}