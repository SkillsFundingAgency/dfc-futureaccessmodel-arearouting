using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;
using MarkEmbling.PostcodesIO.Exceptions;
using Newtonsoft.Json;

namespace DFC.FutureAccessModel.AreaRouting.Providers.Internal
{
    /// <summary>
    /// the fault response provider
    /// </summary>
    internal sealed class FaultResponseProvider :
        IProvideFaultResponses
    {
        /// <summary>
        /// the fault action map
        /// </summary>
        private readonly Dictionary<Type, Func<Exception, HttpResponseMessage>> _faultActionMap = new Dictionary<Type, Func<Exception, HttpResponseMessage>>();

        /// <summary>
        /// initialises an instance of <see cref="FaultResponseProvider"/>
        /// </summary>
        public FaultResponseProvider()
        {
            _faultActionMap.Add(typeof(MalformedRequestException), Fallback);
            _faultActionMap.Add(typeof(NoContentException), Fallback);
            _faultActionMap.Add(typeof(UnprocessableEntityException), UnprocessableEntity);
            _faultActionMap.Add(typeof(AccessForbiddenException), Forbidden);
            _faultActionMap.Add(typeof(UnauthorizedException), Unauthorized);

            _faultActionMap.Add(typeof(PostcodesIOApiException), Fallback);
            _faultActionMap.Add(typeof(PostcodesIOEmptyResponseException), Fallback);
            _faultActionMap.Add(typeof(InvalidPostcodeException), Fallback);
        }

        /// <summary>
        /// get (the) response for...
        /// </summary>
        /// <param name="theException">the exception</param>
        /// <param name="useLoggingScope">use (the) logging scope</param>
        /// <returns>the currently running task containing the http response message</returns>
        public async Task<HttpResponseMessage> GetResponseFor(Exception theException, IScopeLoggingContext useLoggingScope)
        {
            if (_faultActionMap.ContainsKey(theException.GetType()))
            {
                await InformOn(theException, useLoggingScope);
                return _faultActionMap[theException.GetType()].Invoke(theException);
            }

            await useLoggingScope.ExceptionDetail(theException);

            return Fallback(theException);
        }

        /// <summary>
        /// infomr on...
        /// </summary>
        /// <param name="theException">the exception</param>
        /// <param name="useLoggingScope">using (the) logging scope</param>
        /// <returns></returns>
        internal async Task InformOn(Exception theException, IScopeLoggingContext useLoggingScope)
        {
            await useLoggingScope.Information(theException.Message);

            if (It.Has(theException.InnerException))
            {
                await InformOn(theException.InnerException, useLoggingScope);
            }
        }

        /// <summary>
        /// the fallback message
        /// </summary>
        /// <param name="theException">the exception (is ignored in here)</param>
        /// <returns>a 'fallback' message</returns>
        internal HttpResponseMessage Fallback(Exception theException) =>
            new HttpResponseMessage(HttpStatusCode.OK)
                .SetContent(JsonConvert.SerializeObject(RoutingDetail.Default));

        /// <summary>
        /// the malformed request action
        /// </summary>
        /// <returns>a 'bad request' message</returns>
        internal HttpResponseMessage Malformed(Exception theException) =>
            new HttpResponseMessage(HttpStatusCode.BadRequest)
                .SetContent(string.Empty);

        /// <summary>
        /// the forbidden action
        /// </summary>
        /// <returns>a 'forbidden' message</returns>
        internal HttpResponseMessage Forbidden(Exception theException) =>
            new HttpResponseMessage(HttpStatusCode.Forbidden)
                .SetContent(theException.Message);

        /// <summary>
        /// the no content action
        /// </summary>
        /// <returns>a 'no content' message</returns>
        internal HttpResponseMessage NoContent(Exception theException) =>
            new HttpResponseMessage(HttpStatusCode.NoContent)
                .SetContent(theException.Message);

        /// <summary>
        /// the unprocessable entity action
        /// </summary>
        /// <returns>a 'unprocessable entity' message</returns>
        internal HttpResponseMessage UnprocessableEntity(Exception theException) =>
            new HttpResponseMessage(LocalHttpStatusCode.UnprocessableEntity.AsHttpStatusCode())
                .SetContent(theException.Message);

        /// <summary>
        /// the unauthorised action
        /// </summary>
        /// <returns>an 'unauthorised' message</returns>
        internal HttpResponseMessage Unauthorized(Exception theException) =>
            new HttpResponseMessage(HttpStatusCode.Unauthorized)
                .SetContent(string.Empty);

        /// <summary>
        /// the unknown error action
        /// </summary>
        /// <returns>an 'internal server error' message</returns>
        internal HttpResponseMessage UnknownError() =>
            new HttpResponseMessage(HttpStatusCode.InternalServerError)
                .SetContent(string.Empty);
    }
}
