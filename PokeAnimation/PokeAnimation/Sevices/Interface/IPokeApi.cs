using PokeAnimation.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeAnimation.Sevices.Interface
{
    public interface IPokeApi
    {
        #region Methods

        Task<IEnumerable<MonsterResume>> MonsterListAllResumePerRegion(int regionId);

        Task<PokemonList> ObterListaPokemons(int offset = 20, int limit = 20);

        Task<Pokemon> ObterPokemon(string endpoint);

        Task<PokemonType> ObterTiposPokemons();

        Task<IEnumerable<Model.Region>> RegionListAll();

        #endregion Methods
    }
}