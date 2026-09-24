using System;
using System.Collections.Generic;

namespace GPC.Utilities.Extensions
{
    public static class ListExtension
    {
        /// <summary>
        /// Split the list in consecutive chunks of <paramref name="size"/> elements. The last chunk can be smaller
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="size"/> is lower than 1</exception>
        public static List<List<T>> Split<T>(this List<T> listToSplit, int size)
        {
            if (size < 1)
                throw new ArgumentOutOfRangeException(nameof(size), size, "Size must be greater than zero");

            var list = new List<List<T>>();

            for (int i = 0; i < listToSplit.Count; i += size)
                list.Add(listToSplit.GetRange(i, Math.Min(size, listToSplit.Count - i)));

            return list;
        }
    }

    public static class ArrayExtension
    {
        /// <summary>
        /// Split the array in consecutive chunks of <paramref name="size"/> elements. The last chunk can be smaller
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="size"/> is lower than 1</exception>
        public static List<T[]> Split<T>(this T[] arrayToSplit, int size)
        {
            if (size < 1)
                throw new ArgumentOutOfRangeException(nameof(size), size, "Size must be greater than zero");

            var list = new List<T[]>();

            for (int i = 0; i < arrayToSplit.Length; i += size)
            {
                var chunk = new T[Math.Min(size, arrayToSplit.Length - i)];
                Array.Copy(arrayToSplit, i, chunk, 0, chunk.Length);
                list.Add(chunk);
            }

            return list;
        }
    }
}
