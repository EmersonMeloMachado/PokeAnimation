using PokeAnimation.Model;
using PokeAPI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PokeAnimation.Sevices
{
    internal class MonsterRepository
    {
        #region Fields

        private const string IMAGE_BACK = "http://pokeapi.co/media/sprites/pokemon/back/";
        private const string IMAGE_BACK_SHINY = "http://pokeapi.co/media/sprites/pokemon/back/shiny/";
        private const string IMAGE_DEFAULT = "http://pokeapi.co/media/sprites/pokemon/";
        private const string IMAGE_EXTENSION = ".png";
        private const string IMAGE_SHINY = "http://pokeapi.co/media/sprites/pokemon/shiny/";

        #endregion Fields

        #region Methods

        public async Task<IEnumerable<MonsterResume>> ListAllResumePerRegion(int regionId)
        {
            Pokedex pokedex = null;
            //var

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MonsterRepository)).Assembly;
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Sevices.pokedex_{regionId}.json");
            string json = "";
            using(var reader = new System.IO.StreamReader(stream))
            {
                json = await reader.ReadToEndAsync();
            }

            if(!string.IsNullOrEmpty(json))
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

        #endregion Methods
    }
}