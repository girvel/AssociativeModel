using System.Collections.Generic;

namespace AssociativeModel.ConsoleUi
{
    public static class CollectionSelectorHelper
    {
        public static ListSelector<T> ToSelector<T>(this IList<T> list)
        {
            return new ListSelector<T>(list);
        }
    }
}