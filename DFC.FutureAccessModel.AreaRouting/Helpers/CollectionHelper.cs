using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.FutureAccessModel.AreaRouting.Helpers
{
    public static class CollectionHelper
    {
        /// <summary>
        /// For each, to safe list and conducts the action
        /// </summary>
        /// <typeparam name="T">of type</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            It.IsNull(action)
                .AsGuard<ArgumentNullException>();

            var items = collection.SafeList();
            foreach (var item in items)
            {
                action(item);
            }
        }

        /// <summary>
        /// As a safe readonly list.
        /// </summary>
        /// <typeparam name="T">of type</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>
        /// a readonly safe collection
        /// </returns>
        public static IReadOnlyCollection<T> AsSafeReadOnlyList<T>(this IEnumerable<T> list)
        {
            return list.SafeReadOnlyList();
        }

        /// <summary>
        /// As a safe readonly list, task / async compatible
        /// </summary>
        /// <typeparam name="T">of type</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>a readonly safe collection</returns>
        public static async Task<IReadOnlyCollection<T>> AsSafeReadOnlyList<T>(this Task<IEnumerable<T>> list)
        {
            return await list.SafeReadOnlyList();
        }

        /// <summary>
        /// Safe read only list.
        /// </summary>
        /// <typeparam name="T">of type</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>a safe readonly list</returns>
        private static IReadOnlyCollection<T> SafeReadOnlyList<T>(this IEnumerable<T> list)
        {
            return new ReadOnlyCollection<T>(list.SafeList());
        }

        /// <summary>
        /// Safe read only list, task / async compatible
        /// </summary>
        /// <typeparam name="T">of type</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>a safe readonly list</returns>
        private static async Task<IReadOnlyCollection<T>> SafeReadOnlyList<T>(this Task<IEnumerable<T>> list)
        {
            return new ReadOnlyCollection<T>(await list.SafeList());
        }

        /// <summary>
        /// Safe list, the private implementation of null coalescing
        /// </summary>
        /// <typeparam name="T">of type</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>
        /// a safe list
        /// </returns>
        private static List<T> SafeList<T>(this IEnumerable<T> list)
        {
            return (list ?? new List<T>()).ToList();
        }

        /// <summary>
        /// Safe list, the private implementation of null coalescing, task / async compatible
        /// </summary>
        /// <typeparam name="T">of type</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>a safe list</returns>
        private static async Task<List<T>> SafeList<T>(this Task<IEnumerable<T>> list)
        {
            return (await list ?? new List<T>()).ToList();
        }
    }
}
