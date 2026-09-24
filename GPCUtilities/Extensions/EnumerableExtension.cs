using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPC.Utilities.Extensions
{
    public static class EnumerableExtension
    {
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns><see langword="True"/> if all the elements of <paramref name="list1"/> are equals to one element of <paramref name="list2"/> whichever the order is</returns>
        /// <remarks>The hashCode of <typeparamref name="T"/> is used to perform the equality check</remarks>
        public static bool ScrambledEquals<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var cnt = new Dictionary<T, int>();
            foreach (T s in list1)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }
            foreach (T s in list2)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {
                    return false;
                }
            }
            return cnt.Values.All(c => c == 0);
        }

        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <param name="comparer"></param>
        /// <returns><see langword="True"/> if all the elements of <paramref name="list1"/> are equals to one element of <paramref name="list2"/> whichever the order is</returns>
        /// <remarks>The hashCode of <typeparamref name="T"/> is used to perform the equality check</remarks>
        public static bool ScrambledEquals<T>(this IEnumerable<T> list1, IEnumerable<T> list2, IEqualityComparer<T> comparer)
        {
            var cnt = new Dictionary<T, int>(comparer);
            foreach (T s in list1)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }
            foreach (T s in list2)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {
                    return false;
                }
            }
            return cnt.Values.All(c => c == 0);
        }

        /// <returns>The hashcode of the list that depends on the order of the elements</returns>
        public static int GetHashCodeSequence<T>(this IEnumerable<T> list)
        {
            unchecked
            {
                int hashcode = -23;

                foreach (var item in list)
                {
                    // moltiplicando l'hashcode precedente, cambia con l'ordine degli elementi nella lista (a + b) != ( b + a)
                    hashcode = hashcode * -17 + (item == null ? 0 : item.GetHashCode());
                }

                return hashcode; 
            }
        }

        /// <returns>The hashcode of the list that is indipendent from the order of the elements</returns>
        public static int GetHashCodeScrambled<T>(this IEnumerable<T> list)
        {
            unchecked
            {
                int hashcode = -23;

                foreach (var item in list)
                {
                    // non moltiplicando l'hashcode precedente, funziona se la lista è scrambled (a + b) == ( b + a)
                    hashcode += -17 * (item == null ? 0 : item.GetHashCode());
                }

                return hashcode; 
            }
        }

        //https://stackoverflow.com/a/52973907/3884701
        /// <summary>run the <paramref name="funcBody"/> over the <paramref name="source"/> partitioning it to avoid congestion of task fired simustanely</summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="D"></typeparam>
        /// <param name="source"></param>
        /// <param name="funcBody"></param>
        /// <param name="maxDoP">Number of partition of <paramref name="source"/> that will run in paralell. Ideally 12 x Logical Processors or processor number</param>
        /// <returns></returns>        
        public static async Task<IEnumerable<D>> ParallelForEachAsync<T, D>(this IEnumerable<T> source, Func<T, Task<D>> funcBody, int maxDoP = 4)
        {
            async Task<IEnumerable<D>> AwaitPartition(IEnumerator<T> partition)
            {
                using (partition)
                {
                    var results = new List<D>();
                    while (partition.MoveNext())
                    {
                        await Task.Yield();
                        results.Add(await funcBody(partition.Current).ConfigureAwait(false));
                    }

                    return results;
                }
            }

            IEnumerable<D>[] r = await Task.WhenAll(Partitioner.Create(source).GetPartitions(maxDoP).AsParallel().Select(AwaitPartition));

            return r.SelectMany(i => i);
        }

        //https://stackoverflow.com/a/52973907/3884701
        /// <summary>run the <paramref name="funcBody"/> over the <paramref name="source"/> partitioning it to avoid congestion of task fired simustanely</summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="D"></typeparam>
        /// <param name="source"></param>
        /// <param name="funcBody"></param>
        /// <param name="secondInput"></param>
        /// <param name="maxDoP">Number of partition of <paramref name="source"/> that will run in paralell. Ideally 12 x Logical Processors or processor number</param>
        /// <returns></returns>        
        public static async Task<IEnumerable<D>> ParallelForEachAsync<T1, T2, D>(this IEnumerable<T1> source, Func<T1, T2, Task<D>> funcBody, T2 secondInput, int maxDoP = 4)
        {
            async Task<IEnumerable<D>> AwaitPartition(IEnumerator<T1> partition)
            {
                using (partition)
                {
                    var results = new List<D>();
                    while (partition.MoveNext())
                    {
                        await Task.Yield();
                        results.Add(await funcBody(partition.Current, secondInput).ConfigureAwait(false));
                    }

                    return results;
                }
            }

            IEnumerable<D>[] r = await Task.WhenAll(Partitioner.Create(source).GetPartitions(maxDoP).AsParallel().Select(AwaitPartition));

            return r.SelectMany(i => i);
        }
    }
}
