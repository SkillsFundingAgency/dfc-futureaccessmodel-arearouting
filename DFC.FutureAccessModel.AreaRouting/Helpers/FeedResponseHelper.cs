using Microsoft.Azure.Documents.Client;
using System;
using System.Threading.Tasks;

namespace DFC.FutureAccessModel.AreaRouting.Helpers
{
    public static class FeedResponseHelper
    {
        /// <summary>
        /// For each, that waits for the feed response, then conducts the action
        /// </summary>
        /// <typeparam name="T">of type</typeparam>
        /// <param name="response">The response.</param>
        /// <param name="action">The action.</param>
        public static async Task ForEach<T>(this Task<FeedResponse<T>> response, Action<T> action)
        {
            It.IsNull(action)
                .AsGuard<ArgumentNullException>();

            var items = await response;
            foreach (var item in items)
            {
                action(item);
            }
        }

    }
}
