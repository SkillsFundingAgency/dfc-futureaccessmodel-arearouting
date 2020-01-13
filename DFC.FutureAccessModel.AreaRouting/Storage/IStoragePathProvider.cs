using System;

namespace DFC.FutureAccessModel.AreaRouting.Storage
{
    /// <summary>
    /// i provide storage paths
    /// </summary>
    public interface IProvideStoragePaths
    {
        /// <summary>
        /// get the routine detail path for
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <returns>the uri for ther requested storage path</returns>
        Uri GetRoutingDetailResourcePathFor(string theTouchpointID);
    }
}