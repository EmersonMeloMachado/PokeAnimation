using Flurl.Http;
using Newtonsoft.Json;
using PokeAnimation.Model;
using PokeAnimation.Sevices.Interface;
using PokeAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            //var

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MonsterRepository)).Assembly;
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.pokedex_{regionId}.json");
            string json = "";
            if (stream != null)
            {
                using (var reader = new System.IO.StreamReader(stream))
                {
                    json = await reader.ReadToEndAsync();
                }
            }

            if (!string.IsNullOrEmpty(json))
            {
                pokedex = Newtonsoft.Json.JsonConvert.DeserializeObject<Pokedex>(json);
            }
            else
            {
                pokedex = await DataFetcher.GetApiObject<Pokedex>(regionId);
            }

            return pokedex.Entries.Select(x => new MonsterResume
            {
                Id = x.EntryNumber,
                Name = x.Species.Name,
                ImagePath = $"{IMAGE_DEFAULT}{x.EntryNumber}{IMAGE_EXTENSION}"
            });
        }

        public async Task<PokemonList> ObterListaPokemons(int offset = 0, int limit = 20)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) return null;

            try
            {
                var url = $"{EndPoints.BaseUrl}?offset={offset}&limit={limit}";

                var response = await url
                    .AllowAnyHttpStatus()
                    .GetAsync()
                    .ReceiveJson<PokemonList>();

                return response;
            }
            catch (FlurlHttpException ex)
            {
                var msg = await ex.GetResponseStringAsync();
                throw new Exception(msg);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Model.Pokemon> ObterPokemon(string endpoint)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) return null;

            try
            {
                var response = await endpoint
                    .WithTimeout(60)
                    .AllowAnyHttpStatus()
                    .GetAsync();

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var pokemon = JsonConvert.DeserializeObject<Model.Pokemon>(content);
                    return pokemon;
                }
                return null;
            }
            catch (FlurlHttpException ex)
            {
                var msg = await ex.GetResponseStringAsync();
                throw new Exception(msg);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Model.PokemonType> ObterTiposPokemons()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) return null;

            try
            {
                var response = await EndPoints.UrlTypePokemon
                    .WithTimeout(60)
                    .AllowAnyHttpStatus()
                    .GetAsync();

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var pokemon = JsonConvert.DeserializeObject<Model.PokemonType>(content);
                    return pokemon;
                }
                return null;
            }
            catch (FlurlHttpException ex)
            {
                var msg = await ex.GetResponseStringAsync();
                throw new Exception(msg);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Model.Region>> RegionListAll()
        {
            var regions = new List<Model.Region>();
            //var

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MonsterRepository)).Assembly;
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.pokedex.json");
            string json = "";
            if (stream != null)
            {
                using (var reader = new System.IO.StreamReader(stream))
                {
                    json = await reader.ReadToEndAsync();
                }
            }

            regions = JsonConvert.DeserializeObject<List<Model.Region>>(json);

            return regions;
        }

        #endregion Methods
    }
}