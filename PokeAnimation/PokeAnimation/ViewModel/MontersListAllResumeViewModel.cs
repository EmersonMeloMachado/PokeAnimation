using PokeAnimation.Helpers;
using PokeAnimation.Model;
using PokeAnimation.Sevices.Interface;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PokeAnimation.ViewModel
{
    public class MontersListAllResumeViewModel : ObservableObject, IBaseModelView
    {
        #region Fields

        private readonly IPokeApi _repositoryService;
        private ObservableCollection<MonsterResume> _monstersResume;
        private ObservableCollection<Model.Region> _regions;
        private int offSetFinal = 0;
        private int offSetInicial = 0;
        private MonsterResume pokemon;

        private PokemonList pokemonList;

        #endregion Fields

        #region Properties

        public ICommand LoadCommand { get; set; }

        public ObservableCollection<MonsterResume> MonstersResume
        {
            get { return _monstersResume; }
            set { SetProperty(ref _monstersResume, value); }
        }

        public int OffSetFinal
        {
            get { return offSetFinal; }
            set { SetProperty(ref offSetFinal, value); }
        }

        public int OffSetInicial
        {
            get { return offSetInicial; }
            set { SetProperty(ref offSetInicial, value); }
        }

        public MonsterResume Pokemon
        {
            get => this.pokemon;
            set => SetProperty(ref this.pokemon, value);
        }

        public PokemonList PokemonList
        {
            get { return pokemonList; }
            set { SetProperty(ref pokemonList, value); }
        }

        public ObservableCollection<Model.Region> Regions
        {
            get { return _regions; }
            set { SetProperty(ref _regions, value); }
        }

        #endregion Properties

        #region Constructors

        public MontersListAllResumeViewModel(IPokeApi repositoryService)
        {
            this._repositoryService = repositoryService;
            this.LoadCommand = new Command(async () => await LoadPokemons());
            Regions = new ObservableCollection<Model.Region>();
            MonstersResume = new ObservableCollection<MonsterResume>();
        }

        #endregion Constructors

        #region Methods

        private async Task LoadPokemons()
        {
            try
            {
                //IsBusy = true;
                //MockSkeleton();
                this.Regions.Clear();
                var listaRegiao = await this._repositoryService.RegionListAll();
                if (listaRegiao != null)
                {
                    foreach (var i in listaRegiao)
                    {
                        this.Regions.Add(i);
                    }
                }

                this.MonstersResume.Clear();
                foreach (var i in (await this._repositoryService.MonsterListAllResumePerRegion(2)))
                {
                    this.MonstersResume.Add(i);
                }

                //var items = new List<Pokemon>();
                //PokemonList = await _repositoryService.ObterListaPokemons(OffSetInicial, OffSetFinal);

                //if (PokemonList != null)
                //{
                //    foreach (var poke in PokemonList.results)
                //    {
                //        var pokemon = await _repositoryService.ObterPokemon(poke.url);
                //        if (pokemon != null)
                //            items.Add(pokemon);
                //    }

                //    MonstersResume.Clear();
                //    MonstersResume.AddRange(items);
                //    OffSetFinal += 20;
                //}

                //IsBusy = false;
            }
            catch (Exception ex)
            {
                //await _pageDialogService.DisplayAlertAsync("PokeApp", ex.Message, "OK");
            }
            finally
            {
                //IsBusyTypes = false;
                //IsRefreshing = false;
            }
        }

        #endregion Methods
    }
}