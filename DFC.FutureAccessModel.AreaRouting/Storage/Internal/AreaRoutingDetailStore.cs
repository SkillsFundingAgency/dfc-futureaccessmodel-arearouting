using System;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Providers;

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
        /// get...
        /// </summary>
        /// <param name="theTouchpoint">the touchpoint (id)</param>
        /// <returns>an area routing detail (the touchpoint)</returns>
        public async Task<IRoutingDetail> Get(string theTouchpoint)
        {
            var usingPath = StoragePaths.GetRoutingDetailResourcePathFor(theTouchpoint);
            return await DocumentStore.GetDocument<RoutingDetail>(usingPath);
        }

        /// <summary>
        /// add...
        /// </summary>
        /// <param name="theCandidate">the candidate (touchpoint)</param>
        /// <returns>the newly stored routing details (touchpoint)</returns>
        public async Task<IRoutingDetail> Add(IRoutingDetail theCandidate)
        {
            It.IsNull(theCandidate)
                .AsGuard<ArgumentNullException>(nameof(theCandidate));

            var theTouchpoint = theCandidate.TouchpointID;
            It.IsNull(theTouchpoint)
                .AsGuard<ArgumentNullException>(nameof(theTouchpoint));

            var usingPath = StoragePaths.GetRoutingDetailResourcePathFor(theTouchpoint);

            (await DocumentStore.DocumentExists<RoutingDetail>(usingPath))
                .AsGuard<ConflictingResourceException>();

            return await DocumentStore.AddDocument(theCandidate, StoragePaths.RoutingDetailCollection);
        }
    }
}