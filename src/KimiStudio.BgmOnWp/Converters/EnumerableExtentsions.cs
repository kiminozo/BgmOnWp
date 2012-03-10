using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KimiStudio.BgmOnWp.Converters
{
    public static class EnumerableExtentsions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }
    }
}
