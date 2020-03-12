using System.Collections.Generic;

namespace PokeAnimation.Model
{
    public class PokemonList
    {
        #region Properties

        public int count { get; set; }
        public string next { get; set; }
        public string previous { get; set; }
        public List<Result> results { get; set; }

        #endregion Properties
    }

    public class Result
    {
        #region Properties

        public string name { get; set; }
        public string url { get; set; }

        #endregion Properties
    }
}