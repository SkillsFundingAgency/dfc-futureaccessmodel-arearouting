using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;

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
        private Dictionary<Type, Func<Exception, HttpResponseMessage>> _faultActionMap = new Dictionary<Type, Func<Exception, HttpResponseMessage>>();

        /// <summary>
        /// initialises an instance of <see cref="FaultResponseProvider"/>
        /// </summary>
        public FaultResponseProvider()
        {
            _faultActionMap.Add(typeof(MalformedRequestException), Malformed);
            _faultActionMap.Add(typeof(NoContentException), NoContent);
            _faultActionMap.Add(typeof(UnprocessableEntityException), UnprocessableEntity);
            _faultActionMap.Add(typeof(AccessForbiddenException), Forbidden);
            _faultActionMap.Add(typeof(UnauthorizedException), Unauthorized);
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
                await useLoggingScope.Information(theException.Message);
                return _faultActionMap[theException.GetType()].Invoke(theException);
            }

            await useLoggingScope.ExceptionDetail(theException);
            return UnknownError();
        }

        /// <summary>
        /// the malformed request action
        /// </summary>
        /// <returns>a 'bad request' message</returns>
        public HttpResponseMessage Malformed(Exception theException) =>
            new HttpResponseMessage(HttpStatusCode.BadRequest)
                .SetContent(string.Empty);

        /// <summary>
        /// the forbidden action
        /// </summary>
        /// <returns>a 'forbidden' message</returns>
        public HttpResponseMessage Forbidden(Exception theException) =>
            new HttpResponseMessage(HttpStatusCode.Forbidden)
                .SetContent(theException.Message);

        /// <summary>
        /// the no content action
        /// </summary>
        /// <returns>a 'no content' message</returns>
        public HttpResponseMessage NoContent(Exception theException) =>
            new HttpResponseMessage(HttpStatusCode.NoContent)
                .SetContent(theException.Message);

        /// <summary>
        /// the unprocessable entity action
        /// </summary>
        /// <returns>a 'unprocessable entity' message</returns>
        public HttpResponseMessage UnprocessableEntity(Exception theException) =>
            new HttpResponseMessage(LocalHttpStatusCode.UnprocessableEntity.AsHttpStatusCode())
                .SetContent(theException.Message);

        /// <summary>
        /// the unauthorised action
        /// </summary>
        /// <returns>an 'unauthorised' message</returns>
        public HttpResponseMessage Unauthorized(Exception theException) =>
            new HttpResponseMessage(HttpStatusCode.Unauthorized)
                .SetContent(string.Empty);

        /// <summary>
        /// the unknown error action
        /// </summary>
        /// <returns>an 'internal server error' message</returns>
        public HttpResponseMessage UnknownError() =>
            new HttpResponseMessage(HttpStatusCode.InternalServerError)
                .SetContent(string.Empty);
    }
}
