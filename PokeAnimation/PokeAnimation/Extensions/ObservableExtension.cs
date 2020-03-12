using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PokeAnimation.Extensions
{
    public static class ObservableExtension
    {
        #region Methods

        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            items.ToList().ForEach(collection.Add);
        }

        #endregion Methods
    }
}