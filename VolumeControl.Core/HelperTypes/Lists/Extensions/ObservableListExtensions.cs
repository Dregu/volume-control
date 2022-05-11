﻿using VolumeControl.Core.HelperTypes.Lists;

namespace VolumeControl.Core.HelperTypes.Lists.Extensions
{
    public static class ObservableListExtensions
    {
        public static void RemoveAll<T>(this ObservableList<T> collection, Predicate<T> predicate)
        {
            for (int i = collection.Count - 1; i >= 0; --i)
            {
                if (predicate(collection[i]))
                    collection.RemoveAt(i);
            }
        }
    }
}