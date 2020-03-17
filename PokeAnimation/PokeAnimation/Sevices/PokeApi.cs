using Flurl.Http;
using Newtonsoft.Json;
using PokeAnimation.Model;
using PokeAnimation.Sevices.Interface;
using PokeAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PokeAnimation.Sevices
{
    public class PokeApi : IPokeApi
    {
        #region Fields

        private const string IMAGE_BACK = "http://pokeapi.co/media/sprites/pokemon/back/";
        private const string IMAGE_BACK_SHINY = "http://pokeapi.co/media/sprites/pokemon/back/shiny/";
        private const string IMAGE_DEFAULT = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/";
        private const string IMAGE_EXTENSION = ".png";
        private const string IMAGE_SHINY = "http://pokeapi.co/media/sprites/pokemon/shiny/";

        #endregion Fields

        #region Methods

        public async Task<IEnumerable<MonsterResume>> MonsterListAllResumePerRegion(int regionId)
        {
            Pokedex pokedex = null;
            pokedex = await DataFetcher.GetApiObject<Pokedex>(regionId);

            return pokedex.Entries.Select(x => new MonsterResume
            {
                Id = x.EntryNumber,
                Name = x.Species.Name,
                ImagePath = $"{IMAGE_DEFAULT}{x.EntryNumber}{IMAGE_EXTENSION}"
            });
        }

        public async Task<PokemonList> ObterListaPokemons(int offset = 0, int limit = 20)
        {
            if(Connectivity.NetworkAccess != NetworkAccess.Internet) return null;

            try
            {
                var url = $"{EndPoints.BaseUrl}?offset={offset}&limit={limit}";

                var response = await url
                    .AllowAnyHttpStatus()
                    .GetAsync()
                    .ReceiveJson<PokemonList>();

                return response;
            }
            catch(FlurlHttpException ex)
            {
                var msg = await ex.GetResponseStringAsync();
                throw new Exception(msg);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Model.Pokemon> ObterPokemon(string endpoint)
        {
            if(Connectivity.NetworkAccess != NetworkAccess.Internet) return null;

            try
            {
                var response = await endpoint
                    .WithTimeout(60)
                    .AllowAnyHttpStatus()
                    .GetAsync();

                if(response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var pokemon = JsonConvert.DeserializeObject<Model.Pokemon>(content);
                    return pokemon;
                }
                return null;
            }
            catch(FlurlHttpException ex)
            {
                var msg = await ex.GetResponseStringAsync();
                throw new Exception(msg);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Model.PokemonType> ObterTiposPokemons()
        {
            if(Connectivity.NetworkAccess != NetworkAccess.Internet) return null;

            try
            {
                var response = await EndPoints.UrlTypePokemon
                    .WithTimeout(60)
                    .AllowAnyHttpStatus()
                    .GetAsync();

                if(response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var pokemon = JsonConvert.DeserializeObject<Model.PokemonType>(content);
                    return pokemon;
                }
                return null;
            }
            catch(FlurlHttpException ex)
            {
                var msg = await ex.GetResponseStringAsync();
                throw new Exception(msg);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Model.Region>> RegionListAll()
        {
            return new ObservableCollection<Model.Region>()
            {
                new Model.Region {Id = 1,Name =  "national"},
                new Model.Region {Id = 2,Name =  "kanto"},
                new Model.Region {Id = 3,Name =  "original-johto"},
                new Model.Region {Id = 4,Name =  "hoenn"},
                new Model.Region {Id = 5,Name =  "original-sinnoh"},
                new Model.Region {Id = 6,Name =  "extended-sinnoh"},
                new Model.Region {Id = 7,Name =  "updated-johto"},
                new Model.Region {Id = 8,Name =  "original-unova"},
                new Model.Region {Id = 9,Name =  "updated-unova"},
                new Model.Region {Id = 10,Name =  "conquest-gallery"},
                new Model.Region {Id = 11,Name =  "kalos-central"},
                new Model.Region {Id = 12,Name =  "kalos-coastal"},
                new Model.Region {Id = 13,Name =  "kalos-mountain"},
                new Model.Region {Id = 14,Name =  "updated-hoenn"}
            };
        }

        #endregion Methods
    }
}