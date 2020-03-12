using System.Windows.Input;

namespace PokeAnimation.ViewModel
{
    public class PokemonViewModel
    {
        #region Properties

        public ICommand InitCommand { get; set; }
        public ICommand LoadCommand { get; set; }

        #endregion Properties
    }
}