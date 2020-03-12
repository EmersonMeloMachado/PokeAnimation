using System.Collections.Generic;

namespace PokeAnimation.Model
{
    public class PokemonType
    {
        #region Properties

        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<Results> results { get; set; }

        #endregion Properties
    }

    public class Results
    {
        #region Properties

        public string name { get; set; }
        public string url { get; set; }

        #endregion Properties
    }
}