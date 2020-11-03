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
    /// function maps used inside the fault response provider
    /// </summary>
    internal sealed class FunctionMaps : Dictionary<Type, Func<Exception, HttpResponseMessage>> { }

    /// <summary>
    /// methods maps used inside the fault response provider
    /// </summary>
    internal sealed class MethodMaps : Dictionary<TypeOfFunction, FunctionMaps> { }

    /// <summary>
    /// the fault response provider
    /// </summary>
    internal sealed class FaultResponseProvider :
        IProvideFaultResponses
    {
        /// <summary>
        /// the method action map
        /// </summary>
        private readonly MethodMaps _methodMap = new MethodMaps();

        /// <summary>
        /// initialises an instance of <see cref="FaultResponseProvider"/>
        /// </summary>
        public FaultResponseProvider()
        {
            _methodMap.Add(TypeOfFunction.GetByLocation, GetByLocationFaultMap());
            _methodMap.Add(TypeOfFunction.GetByID, DefaultGetFaultMap());
            _methodMap.Add(TypeOfFunction.GetAll, DefaultGetFaultMap());
            _methodMap.Add(TypeOfFunction.Delete, DeleteFaultMap());
            _methodMap.Add(TypeOfFunction.Post, PostFaultMap());
        }

        /// <summary>
        /// get's the 'by location' fault map
        /// </summary>
        /// <returns>a function map</returns>
        public FunctionMaps GetByLocationFaultMap()
        {
            var _faultMap = new FunctionMaps();

            _faultMap.Add(typeof(MalformedRequestException), x => NoContent());
            _faultMap.Add(typeof(NoContentException), x => NoContent(x.Message));

            _faultMap.Add(typeof(InvalidPostcodeException), x => NoContent(x.Message));
            _faultMap.Add(typeof(FallbackActionException), x => NoContent());

            return _faultMap;
        }

        /// <summary>
        /// get's the 'by id' fault map
        /// </summary>
        /// <returns>a function map</returns>
        public FunctionMaps DefaultGetFaultMap()
        {
            var _faultMap = new FunctionMaps();

            _faultMap.Add(typeof(MalformedRequestException), x => Malformed(x.Message));
            _faultMap.Add(typeof(NoContentException), x => NoContent(x.Message));

            _faultMap.Add(typeof(FallbackActionException), x => NoContent(x.Message));

            return _faultMap;
        }

        /// <summary>
        /// the 'delete' fault map
        /// </summary>
        /// <returns>a function map</returns>
        public FunctionMaps DeleteFaultMap()
        {
            var _faultMap = new FunctionMaps();

            _faultMap.Add(typeof(MalformedRequestException), x => Malformed(x.Message));
            _faultMap.Add(typeof(NoContentException), x => NoContent(x.Message));

            _faultMap.Add(typeof(FallbackActionException), x => UnknownError(x.Message));

            return _faultMap;
        }

        /// <summary>
        /// the 'post' fault map
        /// </summary>
        /// <returns>a function map</returns>
        public FunctionMaps PostFaultMap()
        {
            var _faultMap = new FunctionMaps();

            _faultMap.Add(typeof(ConflictingResourceException), x => Conflicted(x.Message));
            _faultMap.Add(typeof(MalformedRequestException), x => Malformed(x.Message));
            _faultMap.Add(typeof(NoContentException), x => NoContent(x.Message));
            _faultMap.Add(typeof(UnprocessableEntityException), x => UnprocessableEntity(x.Message));

            _faultMap.Add(typeof(FallbackActionException), x => UnknownError(x.Message));

            return _faultMap;
        }

        /// <summary>
        /// get (the) response for...
        /// </summary>
        /// <param name="theException">the exception</param>
        /// <param name="theMethod">the type of method</param>
        /// <param name="useLoggingScope">use (the) logging scope</param>
        /// <returns>the currently running task containing the http response message</returns>
        public async Task<HttpResponseMessage> GetResponseFor(Exception theException, TypeOfFunction theMethod, IScopeLoggingContext useLoggingScope)
        {
            var exceptionType = theException.GetType();

            if (_methodMap[theMethod].ContainsKey(exceptionType))
            {
                await InformOn(theException, useLoggingScope);
                return _methodMap[theMethod][exceptionType].Invoke(theException);
            }

            await useLoggingScope.ExceptionDetail(theException);

            return _methodMap[theMethod][typeof(FallbackActionException)].Invoke(theException);
        }

        /// <summary>
        /// inform on...
        /// </summary>
        /// <param name="theException">the exception</param>
        /// <param name="useLoggingScope">using (the) logging scope</param>
        /// <returns>the currently running task</returns>
        internal async Task InformOn(Exception theException, IScopeLoggingContext useLoggingScope)
        {
            await useLoggingScope.Information(theException.Message);

            if (It.Has(theException.InnerException))
            {
                await InformOn(theException.InnerException, useLoggingScope);
            }
        }

        /// <summary>
        /// the malformed request action
        /// </summary>
        /// <returns>a 'bad request' message</returns>
        internal HttpResponseMessage Malformed(string theMessage = "") =>
            CreateMessage(HttpStatusCode.BadRequest)
                .SetContent(theMessage);

        /// <summary>
        /// the conflicted request action
        /// </summary>
        /// <param name="theException">the exception</param>
        /// <returns>a conflicted message</returns>
        internal HttpResponseMessage Conflicted(string theMessage = "") =>
            CreateMessage(HttpStatusCode.Conflict)
                .SetContent(theMessage);

        /// <summary>
        /// the no content action
        /// </summary>
        /// <returns>a 'no content' message</returns>
        internal HttpResponseMessage NoContent(string theMessage = "") =>
            CreateMessage(HttpStatusCode.NoContent)
                .SetContent(theMessage);

        /// <summary>
        /// the unprocessable entity action
        /// </summary>
        /// <returns>a 'unprocessable entity' message</returns>
        internal HttpResponseMessage UnprocessableEntity(string theMessage = "") =>
            CreateMessage(LocalHttpStatusCode.UnprocessableEntity.AsHttpStatusCode())
                .SetContent(theMessage);

        /// <summary>
        /// the unknown error action
        /// </summary>
        /// <returns>an 'internal server error' message</returns>
        internal HttpResponseMessage UnknownError(string theMessage = "") =>
             CreateMessage(HttpStatusCode.InternalServerError)
                .SetContent(theMessage);

        /// <summary>
        /// create's the base http response message
        /// </summary>
        /// <param name="forCode">for (the given) coe</param>
        /// <returns>a http response message</returns>
        internal HttpResponseMessage CreateMessage(HttpStatusCode forCode) =>
            new HttpResponseMessage(forCode);
    }
}
