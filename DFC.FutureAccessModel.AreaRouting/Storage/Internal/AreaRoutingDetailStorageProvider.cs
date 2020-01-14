using System;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;

namespace DFC.FutureAccessModel.AreaRouting.Storage.Internal
{
    /// <summary>
    /// the area routing detail store
    /// </summary>
    internal sealed class AreaRoutingDetailStore :
        IStoreAreaRoutingDetails
    {
        /// <summary>
        /// storage paths
        /// </summary>
        public IProvideStoragePaths StoragePaths { get; }

        /// <summary>
        /// the (underlying) document store
        /// </summary>
        public IStoreDocuments DocumentStore { get; }

        /// <summary>
        /// create an instance of hte <see cref="AreaRoutingDetailStore"/>
        /// </summary>
        /// <param name="paths">the storage paths (provider)</param>
        /// <param name="store">the document store</param>
        public AreaRoutingDetailStore(
            IProvideStoragePaths paths,
            IStoreDocuments store)
        {
            It.IsNull(paths)
                .AsGuard<ArgumentNullException>(nameof(paths));
            It.IsNull(store)
                .AsGuard<ArgumentNullException>(nameof(store));

            StoragePaths = paths;
            DocumentStore = store;
        }

        /// <summary>
        /// get (the) area routing detail for...
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <returns>an area routing detail</returns>
        public async Task<IRoutingDetail> GetAreaRoutingDetailFor(string theTouchpointID)
        {
            // var usingPath = StoragePaths.GetRoutingDetailResourcePathFor(theTouchpointID)
            // return await DocumentStore.GetDocument<RoutingDetail>(usingPath)

            return await Task.FromResult(new RoutingDetail
            {
                TouchpointID = theTouchpointID,
                Area = "Dummy Detail Region Name",
                EmailAddress = "test.address@education.gov.uk",
                SMSNumber = "07123456789",
                TelephoneNumber = "01234567890"
            });
        }
    }
}