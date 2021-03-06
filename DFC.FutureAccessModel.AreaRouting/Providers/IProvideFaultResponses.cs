﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Registration;

namespace DFC.FutureAccessModel.AreaRouting.Providers
{
    /// <summary>
    /// i provide fault responses
    /// </summary>
    public interface IProvideFaultResponses :
        ISupportServiceRegistration
    {
        /// <summary>
        /// get (the) response for...
        /// </summary>
        /// <param name="theException">the exception</param>
        /// <param name="theMethod">the type of method</param>
        /// <param name="useLoggingScope">use (the) logging scope</param>
        /// <returns>the currently running task containing the http response message</returns>
        Task<HttpResponseMessage> GetResponseFor(Exception theException, TypeOfFunction theMethod, IScopeLoggingContext useLoggingScope);
    }
}
