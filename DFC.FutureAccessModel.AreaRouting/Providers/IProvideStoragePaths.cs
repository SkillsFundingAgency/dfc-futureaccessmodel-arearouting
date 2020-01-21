using System;
using DFC.FutureAccessModel.AreaRouting.Registration;

namespace DFC.FutureAccessModel.AreaRouting.Providers
{
    /// <summary>
    /// i provide storage paths
    /// </summary>
    public interface IProvideStoragePaths :
        ISupportServiceRegistration
    {
        /// <summary>
        /// get (the) routine detail path for
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <returns>the uri for the requested storage path</returns>
        Uri GetRoutingDetailResourcePathFor(string theTouchpointID);

        /// <summary>
        /// get (the) local authority resource path for...
        /// </summary>
        /// <param name="theAdminDistrict">the admin district</param>
        /// <returns>the uri for the requested storage path</returns>
        Uri GetLocalAuthorityResourcePathFor(string theAdminDistrict);
    }
}