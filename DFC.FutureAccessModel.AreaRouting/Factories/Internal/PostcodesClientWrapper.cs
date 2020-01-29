using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using MarkEmbling.PostcodesIO;
using MarkEmbling.PostcodesIO.Results;

namespace DFC.FutureAccessModel.AreaRouting.Factories.Internal
{
    /// <summary>
    /// the postcodes client wrapper
    /// </summary>
    internal sealed class PostcodesClientWrapper :
        IWrapPostcodesClient
    {
        /// <summary>
        /// the (actual) client
        /// </summary>
        private readonly IPostcodesIOClient _client;

        /// <summary>
        /// initalises an instance of the <see cref="PostcodesClientWrapper"/>
        /// </summary>
        public PostcodesClientWrapper() =>
            _client = new PostcodesIOClient();

        /// <summary>
        /// look up (async)
        /// </summary>
        /// <param name="thePostcode">the postcode</param>
        /// <returns>the result, which includes ONS and government data</returns>
        public async Task<PostcodeResult> LookupAsync(string thePostcode) =>
            await _client.LookupAsync(thePostcode);

        /// <summary>
        /// lookup outward code (async)
        /// </summary>
        /// <param name="theOutcode">the outward code</param>
        /// <param name="theResultsSize">the (required) result size (defaults to 10)</param>
        /// <returns>a collection of full postcodes (as simple strings)</returns>
        public async Task<IReadOnlyCollection<string>> LookupOutwardCodeAsync(string theOutcode, int theResultsSize = 10) =>
            await _client.AutocompleteAsync(theOutcode, theResultsSize).AsSafeReadOnlyList();

        /// <summary>
        /// validate (async)
        /// </summary>
        /// <param name="thePostcode">the postcode</param>
        /// <returns>true if it's valid (not sure what constitutes 'valid')</returns>
        public async Task<bool> ValidateAsync(string thePostcode) =>
            await _client.ValidateAsync(thePostcode);
    }
}
