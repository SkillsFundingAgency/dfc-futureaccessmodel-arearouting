using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Storage;
using DFC.FutureAccessModel.AreaRouting.Wrappers;

namespace DFC.FutureAccessModel.AreaRouting.Providers.Internal
{
    /// <summary>
    /// the expression action provider
    /// </summary>
    public sealed class ExpressionActionProvider :
        IProvideExpressionActions
    {
        /// <summary>
        /// the action maps
        /// </summary>
        public IReadOnlyDictionary<TypeOfExpression, Func<string, IScopeLoggingContext, Task<string>>> ActionMap { get; }

        /// <summary>
        /// the postcode (client)
        /// </summary>
        public IWrapPostcodesClient Postcode { get; }

        /// <summary>
        /// the local authority storage client
        /// </summary>
        public IStoreLocalAuthorities Authority { get; }

        /// <summary>
        /// instantiates an instance of <see cref="ExpressionActionProvider"/> 
        /// </summary>
        /// <param name="postcode">the postcode client</param>
        public ExpressionActionProvider(
            ICreatePostcodeClients factory,
            IStoreLocalAuthorities authorityProvider)
        {
            It.IsNull(factory)
                .AsGuard<ArgumentNullException>(nameof(factory));
            It.IsNull(authorityProvider)
                .AsGuard<ArgumentNullException>(nameof(authorityProvider));

            Postcode = factory.CreateClient();
            Authority = authorityProvider;
            ActionMap = new Dictionary<TypeOfExpression, Func<string, IScopeLoggingContext, Task<string>>>()
            {
                [TypeOfExpression.Town] = GetTouchpointIDFromTown,
                [TypeOfExpression.Outward] = GetTouchpointIDFromOutwardCode,
                [TypeOfExpression.Postcode] = GetTouchpointIDFromPostcode
            };
        }

        /// <summary>
        ///  get action for
        /// </summary>
        /// <param name="theExpressionType">the expression type</param>
        /// <returns>the action for the exression type or throws <seealso cref="MalformedRequestException"/> for unknown types</returns>
        public Func<string, IScopeLoggingContext, Task<string>> GetActionFor(TypeOfExpression theExpressionType) =>
            ActionMap.ContainsKey(theExpressionType)
                ? ActionMap[theExpressionType]
                : DefaultAction();

        /// <summary>
        /// the default action for unkown types
        /// </summary>
        /// <param name="forExpression">for the expression</param>
        /// <returns>nothing, it just throws</returns>
        public Func<string, IScopeLoggingContext, Task<string>> DefaultAction() =>
            UnknownCandidateTypeAction;

        /// <summary>
        /// this is the default unknown candidate type action
        /// </summary>
        /// <param name="theCandidate">the candidate</param>
        /// <param name="inScope">in scope</param>
        /// <param name="forExpression">for (the) expression</param>
        /// <returns>nothing, this should throw</returns>
        public async Task<string> UnknownCandidateTypeAction(string theCandidate, IScopeLoggingContext inScope)
        {
            await inScope.EnterMethod();

            await inScope.Information($"malformed request candidate: '{theCandidate}'");

            await inScope.ExitMethod();

            throw new MalformedRequestException(theCandidate);
        }

        /// <summary>
        /// get (the) touchpoint id from (the) town
        /// </summary>
        /// <param name="theCandidate">the candidate</param>
        /// <param name="inScope">in (the logging) scope</param>
        /// <returns>the LAD code</returns>
        public async Task<string> GetTouchpointIDFromTown(string theCandidate, IScopeLoggingContext inScope)
        {
            await Task.CompletedTask;
            throw new NotSupportedException("GetTouchpointIDFromTown: this operation has not yet been coded");
        }

        /// <summary>
        /// get (the) touchpoint id using the outward code
        /// </summary>
        /// <param name="theCandidate">the candidate</param>
        /// <param name="inScope">in (the logging) scope</param>
        /// <returns>the LAD code</returns>
        public async Task<string> GetTouchpointIDFromOutwardCode(string theCandidate, IScopeLoggingContext inScope)
        {
            await inScope.EnterMethod();

            var candidate = await GetPostcodeUsing(theCandidate, inScope);

            await inScope.ExitMethod();

            return await GetTouchpointIDFromPostcode(candidate, inScope);
        }

        /// <summary>
        /// get (the) touchpoint id from (the) Postcode
        /// </summary>
        /// <param name="theCandidate">the candidate</param>
        /// <param name="inScope">in (the logging) scope</param>
        /// <returns>the LAD code</returns>
        public async Task<string> GetTouchpointIDFromPostcode(string theCandidate, IScopeLoggingContext inScope)
        {
            await inScope.EnterMethod();
            await inScope.Information($"seeking postcode '{theCandidate}'");

            var result = await Postcode.LookupAsync(theCandidate);

            if (It.IsNull(result))
            {
                await inScope.Information($"postcode search failed for: '{theCandidate}'");

                var theOutwardCode = GetOutwardCodeFrom(theCandidate);
                var theNewCandidate = await GetPostcodeUsing(theOutwardCode, inScope);

                result = await Postcode.LookupAsync(theNewCandidate);
            }

            It.IsNull(result)
                .AsGuard<InvalidPostcodeException>();

            await inScope.Information($"found postcode for '{result.Postcode}'");
            await inScope.Information($"seeking local authority '{result.Codes.AdminDistrict}'");

            var authority = await Authority.Get(result.Codes.AdminDistrict);

            await inScope.Information($"found local authority '{authority.LADCode}'");
            await inScope.ExitMethod();

            return authority.TouchpointID;
        }

        /// <summary>
        /// get postcode from outward code using...
        /// </summary>
        /// <param name="theOutwardCode">the outward code</param>
        /// <param name="inScope">in scope</param>
        /// <returns>the currently running task and the (full) postcode candidate</returns>
        public async Task<string> GetPostcodeUsing(string theOutwardCode, IScopeLoggingContext inScope)
        {
            await inScope.EnterMethod();

            It.IsEmpty(theOutwardCode)
                .AsGuard<ArgumentNullException>(nameof(theOutwardCode));

            await inScope.Information($"seeking postcode via outward code: '{theOutwardCode}'");

            var result = await Postcode.LookupOutwardCodeAsync(theOutwardCode);

            It.IsEmpty(result)
                .AsGuard<NoContentException>();

            await inScope.ExitMethod();

            return result.FirstOrDefault();
        }

        /// <summary>
        /// get outward code from...
        /// </summary>
        /// <param name="thisPostcode">this postcode</param>
        /// <returns>the first part of the postcode, assumes the postcode is already legit</returns>
        public string GetOutwardCodeFrom(string thisPostcode) =>
            thisPostcode.Contains(" ")
                ? thisPostcode.Substring(0, thisPostcode.IndexOf(" ")).Trim()
                : thisPostcode.Substring(0, thisPostcode.Length - 3).Trim();

    }
}
