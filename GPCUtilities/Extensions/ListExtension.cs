using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPC.Utilities.Extensions
{
    public static class ListExtension
    {
        public static List<List<T>> Split<T>(this List<T> listToSplit, int size)
        {
            var list = new List<List<T>>();

            for (int i = 0; i < listToSplit.Count; i += size)
                list.Add(listToSplit.GetRange(i, Math.Min(size, listToSplit.Count - i)));

            return list;
        }
    }

    public static class ArrayExtension
    {
        public static List<T[]> Split<T>(this T[] arrayToSplit, int size)
        {
            var list = new List<T[]>();

            for (var i = 0; i < (float)arrayToSplit.Length / size; i++)
            {
                list.Add(arrayToSplit.Skip(i * size).Take(size).ToArray());
            }

            return list;
        }
    }
}
