using System.Collections.Generic;
using System.Threading.Tasks;
using MarkEmbling.PostcodesIO.Results;

namespace DFC.FutureAccessModel.AreaRouting.Factories
{
    /// <summary>
    /// i wrap (the) postcodes client
    /// </summary>
    public interface IWrapPostcodesClient
    {
        /// <summary>
        /// look up (async)
        /// </summary>
        /// <param name="thePostcode">the postcode</param>
        /// <returns>the result, which includes ONS and government data</returns>
        Task<PostcodeResult> LookupAsync(string thePostcode);

        /// <summary>
        /// lookup outward code (async)
        /// </summary>
        /// <param name="theOutcode">the outward code</param>
        /// <param name="theResultsSize">the (required) result size (defaults to 10)</param>
        /// <returns>a collection of full postcodes (as simple strings)</returns>

        Task<IReadOnlyCollection<string>> LookupOutwardCodeAsync(string theOutcode, int theResultsSize = 10);

        /// <summary>
        /// validate (async)
        /// </summary>
        /// <param name="thePostcode">the postcode</param>
        /// <returns>true if it's valid (not sure what constitutes 'valid')</returns>
        Task<bool> ValidateAsync(string thePostcode);
    }
}
