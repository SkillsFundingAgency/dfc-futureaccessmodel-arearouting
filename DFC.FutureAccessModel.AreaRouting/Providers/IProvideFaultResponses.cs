using System;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFC.FutureAccessModel.AreaRouting.Providers
{
    /// <summary>
    /// i provide fault responses
    /// </summary>
    public interface IProvideFaultResponses
    {
        /// <summary>
        /// get (the) response for...
        /// </summary>
        /// <param name="theException">the exception</param>
        /// <param name="theMethod">the type of method</param>
        /// <param name="useLoggingScope">use (the) logging scope</param>
        /// <returns>the currently running task containing the http response message</returns>
        Task<IActionResult> GetResponseFor(Exception theException, TypeOfFunction theMethod, IScopeLoggingContext useLoggingScope);
    }
}
