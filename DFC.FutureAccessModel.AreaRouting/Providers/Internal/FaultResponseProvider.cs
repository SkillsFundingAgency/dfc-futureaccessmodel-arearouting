using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFC.FutureAccessModel.AreaRouting.Providers.Internal
{
    /// <summary>
    /// function maps used inside the fault response provider
    /// </summary>
    internal sealed class FunctionMaps : Dictionary<Type, Func<Exception, IActionResult>> { }

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

            _faultMap.Add(typeof(MalformedRequestException), x => new NoContentResult());
            _faultMap.Add(typeof(NoContentException), x => new NoContentResult());

            _faultMap.Add(typeof(InvalidPostcodeException), x => new NoContentResult());
            _faultMap.Add(typeof(FallbackActionException), x => new NoContentResult());

            return _faultMap;
        }

        /// <summary>
        /// get's the 'by id' fault map
        /// </summary>
        /// <returns>a function map</returns>
        public FunctionMaps DefaultGetFaultMap()
        {
            var _faultMap = new FunctionMaps();

            _faultMap.Add(typeof(MalformedRequestException), x => new BadRequestObjectResult(x.Message));
            _faultMap.Add(typeof(NoContentException), x => new NoContentResult());

            _faultMap.Add(typeof(FallbackActionException), x => new NoContentResult());

            return _faultMap;
        }

        /// <summary>
        /// the 'delete' fault map
        /// </summary>
        /// <returns>a function map</returns>
        public FunctionMaps DeleteFaultMap()
        {
            var _faultMap = new FunctionMaps();

            _faultMap.Add(typeof(MalformedRequestException), x => new BadRequestObjectResult(new { x.Message}));
            _faultMap.Add(typeof(NoContentException), x => new NoContentResult());

            _faultMap.Add(typeof(FallbackActionException), x => new InternalServerErrorResult());

            return _faultMap;
        }

        /// <summary>
        /// the 'post' fault map
        /// </summary>
        /// <returns>a function map</returns>
        public FunctionMaps PostFaultMap()
        {
            var _faultMap = new FunctionMaps();

            _faultMap.Add(typeof(ConflictingResourceException), x => new ConflictObjectResult(x.Message));
            _faultMap.Add(typeof(MalformedRequestException), x => new BadRequestObjectResult(x.Message));
            _faultMap.Add(typeof(NoContentException), x => new NoContentResult());
            _faultMap.Add(typeof(UnprocessableEntityException), x => new UnprocessableEntityObjectResult(x.Message));

            _faultMap.Add(typeof(FallbackActionException), x => new InternalServerErrorResult());

            return _faultMap;
        }

        /// <summary>
        /// get (the) response for...
        /// </summary>
        /// <param name="theException">the exception</param>
        /// <param name="theMethod">the type of method</param>
        /// <param name="useLoggingScope">use (the) logging scope</param>
        /// <returns>the currently running task containing the http response message</returns>
        public async Task<IActionResult> GetResponseFor(Exception theException, TypeOfFunction theMethod, IScopeLoggingContext useLoggingScope)
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
    }
}
