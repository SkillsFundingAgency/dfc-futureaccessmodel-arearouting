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
        private readonly IDictionary<TypeOfExpression, Func<string, IScopeLoggingContext, Task<string>>> _actionMap = new Dictionary<TypeOfExpression, Func<string, IScopeLoggingContext, Task<string>>>();

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

            _actionMap.Add(TypeOfExpression.Town, GetTouchpointIDFromTown);
            _actionMap.Add(TypeOfExpression.Outward, GetTouchpointIDFromOutwardCode);
            _actionMap.Add(TypeOfExpression.Postcode, GetTouchpointIDFromPostcode);
        }

        /// <summary>
        ///  get action for
        /// </summary>
        /// <param name="theExpressionType">the expression type</param>
        /// <returns>the action for the exression type or throws <seealso cref="MalformedRequestException"/> for unknown types</returns>
        public Func<string, IScopeLoggingContext, Task<string>> GetActionFor(TypeOfExpression theExpressionType) =>
            _actionMap.ContainsKey(theExpressionType)
                ? _actionMap[theExpressionType]
                : DefaultAction(theExpressionType);

        /// <summary>
        /// the default action for unkown types
        /// </summary>
        /// <param name="forExpression">for the expression</param>
        /// <returns>nothing, it just throws</returns>
        public Func<string, IScopeLoggingContext, Task<string>> DefaultAction(TypeOfExpression forExpression)
        {
            throw new MalformedRequestException(nameof(forExpression));
        }

        /// <summary>
        /// get (the) touchpoint id from (the) town
        /// </summary>
        /// <param name="theCandidate">the candidate</param>
        /// <param name="usingScope">using (the logging) scope</param>
        /// <returns>the LAD code</returns>
        public async Task<string> GetTouchpointIDFromTown(string theCandidate, IScopeLoggingContext usingScope)
        {
            await Task.CompletedTask;
            throw new NotSupportedException("GetTouchpointIDFromTown: this operation has not yet been coded");
        }

        /// <summary>
        /// get (the) touchpoint id using the outward code
        /// </summary>
        /// <param name="theCandidate">the candidate</param>
        /// <param name="usingScope">using (the logging) scope</param>
        /// <returns>the LAD code</returns>
        public async Task<string> GetTouchpointIDFromOutwardCode(string theCandidate, IScopeLoggingContext usingScope)
        {
            await usingScope.EnterMethod();

            It.IsNull(theCandidate)
                .AsGuard<ArgumentNullException>(nameof(theCandidate));

            await usingScope.Information($"seeking postcode via outward code: '{theCandidate}'");

            var result = await Postcode.LookupOutwardCodeAsync(theCandidate);

            It.IsEmpty(result)
                .AsGuard<MalformedRequestException>();

            await usingScope.ExitMethod();

            return await GetTouchpointIDFromPostcode(result.FirstOrDefault(), usingScope);
        }

        /// <summary>
        /// get (the) touchpoint id from (the) Postcode
        /// </summary>
        /// <param name="theCandidate">the candidate</param>
        /// <param name="usingScope">using (the logging) scope</param>
        /// <returns>the LAD code</returns>
        public async Task<string> GetTouchpointIDFromPostcode(string theCandidate, IScopeLoggingContext usingScope)
        {
            await usingScope.EnterMethod();
            await usingScope.Information($"seeking postcode '{theCandidate}'");

            var result = await Postcode.LookupAsync(theCandidate);

            It.IsNull(result)
                .AsGuard<InvalidPostcodeException>();

            await usingScope.Information($"found postcode for '{result.OutCode} {result.InCode}'");
            await usingScope.Information($"seeking local authority '{result.Codes.AdminDistrict}'");

            var authority = await Authority.Get(result.Codes.AdminDistrict);

            await usingScope.Information($"found local authority '{authority.LADCode}'");
            await usingScope.ExitMethod();

            return authority.TouchpointID;
        }
    }
}
