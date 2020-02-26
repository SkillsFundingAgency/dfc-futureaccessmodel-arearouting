using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Storage;

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

        public IStoreAreaRoutingDetails Store { get; }

        /// <summary>
        /// initialises an instance of <see cref="FaultResponseProvider"/>
        /// </summary>
        /// <param name="store">the routing detail 'fallback' store</param>
        public FaultResponseProvider(IStoreAreaRoutingDetails store)
        {
            It.IsNull(store)
                .AsGuard<ArgumentNullException>(nameof(store));

            Store = store;

            _methodMap.Add(TypeOfFunction.GetByLocation, GetByLocationFaultMap());
            _methodMap.Add(TypeOfFunction.GetByID, GetByIDFaultMap());
            _methodMap.Add(TypeOfFunction.GetAll, GetAllFaultMap());
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

            _faultMap.Add(typeof(MalformedRequestException), Fallback);
            _faultMap.Add(typeof(NoContentException), Fallback);
            _faultMap.Add(typeof(UnprocessableEntityException), UnprocessableEntity);
            _faultMap.Add(typeof(AccessForbiddenException), Forbidden);
            _faultMap.Add(typeof(UnauthorizedException), Unauthorized);

            _faultMap.Add(typeof(InvalidPostcodeException), Fallback);
            _faultMap.Add(typeof(FallbackActionException), Fallback);

            return _faultMap;
        }

        /// <summary>
        /// get's the 'by id' fault map
        /// </summary>
        /// <returns>a function map</returns>
        public FunctionMaps GetByIDFaultMap()
        {
            var _faultMap = new FunctionMaps();

            _faultMap.Add(typeof(MalformedRequestException), Malformed);
            _faultMap.Add(typeof(NoContentException), NoContent);
            _faultMap.Add(typeof(UnprocessableEntityException), UnprocessableEntity);
            _faultMap.Add(typeof(AccessForbiddenException), Forbidden);
            _faultMap.Add(typeof(UnauthorizedException), Unauthorized);

            _faultMap.Add(typeof(FallbackActionException), NoContent);

            return _faultMap;
        }

        /// <summary>
        /// get's the 'all id's' fault map
        /// </summary>
        /// <returns>a function map</returns>
        public FunctionMaps GetAllFaultMap()
        {
            var _faultMap = new FunctionMaps();

            _faultMap.Add(typeof(MalformedRequestException), Malformed);
            _faultMap.Add(typeof(NoContentException), NoContent);
            _faultMap.Add(typeof(UnprocessableEntityException), UnprocessableEntity);
            _faultMap.Add(typeof(AccessForbiddenException), Forbidden);
            _faultMap.Add(typeof(UnauthorizedException), Unauthorized);

            _faultMap.Add(typeof(FallbackActionException), CollectionFallback);

            return _faultMap;
        }

        /// <summary>
        /// the 'delete' fault map
        /// </summary>
        /// <returns>a function map</returns>
        public FunctionMaps DeleteFaultMap()
        {
            var _faultMap = new FunctionMaps();

            _faultMap.Add(typeof(MalformedRequestException), Malformed);
            _faultMap.Add(typeof(NoContentException), NoContent);
            _faultMap.Add(typeof(UnprocessableEntityException), UnprocessableEntity);
            _faultMap.Add(typeof(AccessForbiddenException), Forbidden);
            _faultMap.Add(typeof(UnauthorizedException), Unauthorized);

            _faultMap.Add(typeof(FallbackActionException), UnknownError);

            return _faultMap;
        }

        /// <summary>
        /// the 'post' fault map
        /// </summary>
        /// <returns>a function map</returns>
        public FunctionMaps PostFaultMap()
        {
            var _faultMap = new FunctionMaps();

            _faultMap.Add(typeof(ConflictingResourceException), Conflicted);
            _faultMap.Add(typeof(MalformedRequestException), Malformed);
            _faultMap.Add(typeof(NoContentException), NoContent);
            _faultMap.Add(typeof(UnprocessableEntityException), UnprocessableEntity);
            _faultMap.Add(typeof(AccessForbiddenException), Forbidden);
            _faultMap.Add(typeof(UnauthorizedException), Unauthorized);

            _faultMap.Add(typeof(FallbackActionException), UnknownError);

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
            if (_methodMap[theMethod].ContainsKey(theException.GetType()))
            {
                await InformOn(theException, useLoggingScope);
                return _methodMap[theMethod][theException.GetType()].Invoke(theException);
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
        /// gets the latest version of the fallback values
        /// </summary>
        /// <returns></returns>
        internal IRoutingDetail GetFallbackValue() =>
            Store.Get(RoutingDetail.FallbackID).Result;

        /// <summary>
        /// the fallback message
        /// </summary>
        /// <param name="theException">the exception (is ignored in here)</param>
        /// <returns>a 'fallback' message</returns>
        internal HttpResponseMessage Fallback(Exception theException) =>
            CreateMessage(HttpStatusCode.OK)
                .SetContent(GetFallbackValue());

        /// <summary>
        /// the collection fallback ??
        /// </summary>
        /// <param name="theException">the exception (is ignored in here)</param>
        /// <returns>a 'collection fallback' message</returns>
        internal HttpResponseMessage CollectionFallback(Exception theException) =>
            CreateMessage(HttpStatusCode.OK)
                .SetContent("{ [ ] }");

        /// <summary>
        /// the malformed request action
        /// </summary>
        /// <returns>a 'bad request' message</returns>
        internal HttpResponseMessage Malformed(Exception theException) =>
            CreateMessage(HttpStatusCode.BadRequest)
                .SetContent(string.Empty);

        /// <summary>
        /// the conflicted request action
        /// </summary>
        /// <param name="theException">the exception</param>
        /// <returns>a conflicted message</returns>
        internal HttpResponseMessage Conflicted(Exception theException) =>
            CreateMessage(HttpStatusCode.Conflict)
                .SetContent(string.Empty);

        /// <summary>
        /// the forbidden action
        /// </summary>
        /// <returns>a 'forbidden' message</returns>
        internal HttpResponseMessage Forbidden(Exception theException) =>
            CreateMessage(HttpStatusCode.Forbidden)
                .SetContent(theException.Message);

        /// <summary>
        /// the no content action
        /// </summary>
        /// <returns>a 'no content' message</returns>
        internal HttpResponseMessage NoContent(Exception theException) =>
            CreateMessage(HttpStatusCode.NoContent)
                .SetContent(theException.Message);

        /// <summary>
        /// the unprocessable entity action
        /// </summary>
        /// <returns>a 'unprocessable entity' message</returns>
        internal HttpResponseMessage UnprocessableEntity(Exception theException) =>
            CreateMessage(LocalHttpStatusCode.UnprocessableEntity.AsHttpStatusCode())
                .SetContent(theException.Message);

        /// <summary>
        /// the unauthorised action
        /// </summary>
        /// <returns>an 'unauthorised' message</returns>
        internal HttpResponseMessage Unauthorized(Exception theException) =>
            CreateMessage(HttpStatusCode.Unauthorized)
                .SetContent(string.Empty);

        /// <summary>
        /// the unknown error action
        /// </summary>
        /// <returns>an 'internal server error' message</returns>
        internal HttpResponseMessage UnknownError(Exception theException) =>
             CreateMessage(HttpStatusCode.InternalServerError)
                .SetContent(theException?.Message ?? string.Empty);

        /// <summary>
        /// create's the base http response message
        /// </summary>
        /// <param name="forCode">for (the given) coe</param>
        /// <returns>a http response message</returns>
        internal HttpResponseMessage CreateMessage(HttpStatusCode forCode) =>
            new HttpResponseMessage(forCode);
    }
}
