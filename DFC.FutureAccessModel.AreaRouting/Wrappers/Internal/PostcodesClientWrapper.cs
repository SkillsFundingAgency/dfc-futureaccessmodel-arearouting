using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using MarkEmbling.PostcodesIO;
using MarkEmbling.PostcodesIO.Results;

namespace DFC.FutureAccessModel.AreaRouting.Wrappers.Internal
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
        public IPostcodesIOClient Client { get; }

        /// <summary>
        /// initialises an instance of the <see cref="PostcodesClientWrapper"/>
        /// </summary>
        public PostcodesClientWrapper() : this(new PostcodesIOClient()) { }

        /// <summary>
        /// initialises an instance of the <see cref="PostcodesClientWrapper"/>
        /// using the principle of poor man's DI
        /// </summary>
        /// <param name="theClient">the client</param>
        public PostcodesClientWrapper(IPostcodesIOClient theClient)
        {
            It.IsNull(theClient)
                .AsGuard<ArgumentNullException>(nameof(theClient));

            Client = theClient;
        }

        /// <summary>
        /// look up (async)
        /// </summary>
        /// <param name="thePostcode">the postcode</param>
        /// <returns>the result, which includes ONS and government data</returns>
        public async Task<PostcodeResult> LookupAsync(string thePostcode) =>
            await Client.LookupAsync(thePostcode);

        /// <summary>
        /// lookup outward code (async)
        /// </summary>
        /// <param name="theOutcode">the outward code</param>
        /// <param name="theResultsSize">the (required) result size (defaults to 10)</param>
        /// <returns>a collection of full postcodes (as simple strings)</returns>
        public async Task<IReadOnlyCollection<string>> LookupOutwardCodeAsync(string theOutcode, int theResultsSize = 10) =>
            await Client.AutocompleteAsync(theOutcode, theResultsSize).AsSafeReadOnlyList();

        /// <summary>
        /// validate (async)
        /// </summary>
        /// <param name="thePostcode">the postcode</param>
        /// <returns>true if it's valid (not sure what constitutes 'valid')</returns>
        public async Task<bool> ValidateAsync(string thePostcode) =>
            await Client.ValidateAsync(thePostcode);
    }
}
