using System;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Providers;
using DFC.FutureAccessModel.AreaRouting.Storage;
using DFC.HTTP.Standard;
using Newtonsoft.Json;

namespace DFC.FutureAccessModel.AreaRouting.Adapters.Internal
{
    internal sealed class LocalAuthorityManagmentFunctionAdapter :
        GeneralFunctionAdapter,
        IManageLocalAuthorities
    {
        /// <summary>
        /// i store local authorities
        /// </summary>
        public IStoreLocalAuthorities Authorities { get; }

        /// <summary>
        /// instantiates a new instance of <see cref="LocalAuthorityManagmentFunctionAdapter"/>
        /// </summary>
        /// <param name="responseHelper"></param>
        /// <param name="faultResponses"></param>
        /// <param name="safeOperations"></param>
        /// <param name="authorities"></param>
        public LocalAuthorityManagmentFunctionAdapter(
            IHttpResponseMessageHelper responseHelper,
            IProvideFaultResponses faultResponses,
            IProvideSafeOperations safeOperations,
            IStoreLocalAuthorities authorities) :
                base(responseHelper, faultResponses, safeOperations)
        {
            It.IsNull(authorities)
                .AsGuard<ArgumentNullException>(nameof(authorities));

            Authorities = authorities;
        }

        /// <summary>
        /// get (the) local authority for...
        /// </summary>
        /// <param name="theTouchpoint">the touchpoint</param>
        /// <param name="theLADCode">the local adinistrative district code</param>
        /// <param name="inScope">in scope</param>
        /// <returns>the result of the operation</returns>
        public async Task<HttpResponseMessage> GetAuthorityFor(string theTouchpoint, string theLADCode, IScopeLoggingContext inScope) =>
            await SafeOperations.Try(
                () => ProcessGetAuthorityFor(theTouchpoint, theLADCode, inScope),
                x => Faults.GetResponseFor(x, TypeofMethod.Post, inScope));

        public async Task<HttpResponseMessage> ProcessGetAuthorityFor(string theTouchpoint, string theLADCode, IScopeLoggingContext inScope)
        {
            await inScope.EnterMethod();

            var result = await Authorities.Get(theLADCode);

            (result.TouchpointID != theTouchpoint)
                .AsGuard<NoContentException>();

            await inScope.ExitMethod();

            return Respond.Ok().SetContent(result);
        }

        /// <summary>
        /// add new authority for...
        /// </summary>
        /// <param name="theTouchpoint">the touchpoint</param>
        /// <param name="usingContent">using content</param>
        /// <param name="inScope">in scope</param>
        /// <returns>the result of the operation</returns>
        public async Task<HttpResponseMessage> AddNewAuthorityFor(
            string theTouchpoint,
            string usingContent,
            IScopeLoggingContext inScope) =>
            await SafeOperations.Try(
                () => ProcessAddNewAuthorityFor(theTouchpoint, usingContent, inScope),
                x => Faults.GetResponseFor(x, TypeofMethod.Post, inScope));

        /// <summary>
        /// process, add new authority for...
        /// submission choices...
        /// { "authorities":[
        ///     {"LADCode": "E00060060", "Name": "Widdicombe Sands" },
        ///     {"LADCode": "E00060061", "Name": "WhiteSands" },
        ///     {"LADCode": "E00060062", "Name": "Poppit Sands" },
        ///     {"LADCode": "E00060063", "Name": "Morecombe Sands" }
        ///     ]}
        /// {"LADCode": "E00060060", "Name": "Widdicombe Sands" }
        /// {"TouchpointID":"0000000102", "LADCode": "E00060060", "Name": "Widdicombe Sands" }
        /// </summary>
        /// <param name="theTouchpoint">the touchpoint</param>
        /// <param name="usingContent">using content</param>
        /// <param name="inScope">in scope</param>
        /// <returns>the result of the operation</returns>
        public async Task<HttpResponseMessage> ProcessAddNewAuthorityFor(
            string theTouchpoint,
            string usingContent,
            IScopeLoggingContext inScope)
        {
            await inScope.EnterMethod();

            var candidate = JsonConvert.DeserializeObject<LocalAuthority>(usingContent);

            if (It.IsEmpty(candidate.TouchpointID))
            {
                candidate.TouchpointID = theTouchpoint;
            }

            var result = await Authorities.Add(candidate);

            await inScope.ExitMethod();

            return Respond.Created().SetContent(result);
        }
    }
}
