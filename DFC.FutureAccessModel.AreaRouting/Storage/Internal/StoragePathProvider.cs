using System;
using DFC.FutureAccessModel.AreaRouting.Providers;
using Microsoft.Azure.Documents.Client;

namespace DFC.FutureAccessModel.AreaRouting.Storage.Internal
{
    /// <summary>
    /// the storage path provider
    /// </summary>
    internal sealed class StoragePathProvider :
        IProvideStoragePaths
    {
        public const string DocumentStoreIDKey = "DocumentStoreID";

        public const string RoutingDetailCollectionIDKey = "RoutingDetailCollectionID";

        public const string LocalAuthorityCollectionIDKey = "LocalAuthorityCollectionID";

        public Uri RoutingDetailCollection { get; }

        public Uri LocalAuthorityCollection { get; }

        // TODO: find out how these environment variables are created
        public string DocumentStoreID { get; }

        public string RoutingDetailCollectionID { get; }

        public string LocalAuthorityCollectionID { get; }

        /// <summary>
        /// initialises the storage path provider
        /// </summary>
        public StoragePathProvider(IProvideApplicationSettings usingEnvironment)
        {
            DocumentStoreID = usingEnvironment.GetVariable(DocumentStoreIDKey);
            RoutingDetailCollectionID = usingEnvironment.GetVariable(RoutingDetailCollectionIDKey);
            LocalAuthorityCollectionID = usingEnvironment.GetVariable(LocalAuthorityCollectionIDKey);

            RoutingDetailCollection = UriFactory.CreateDocumentCollectionUri(DocumentStoreID, RoutingDetailCollectionID);
            LocalAuthorityCollection = UriFactory.CreateDocumentCollectionUri(DocumentStoreID, LocalAuthorityCollectionID);
        }

        /// <summary>
        /// get the routine detail path for
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <returns>the uri for ther requested storage path</returns>
        public Uri GetRoutingDetailResourcePathFor(string theTouchpointID) =>
            UriFactory.CreateDocumentUri(DocumentStoreID, RoutingDetailCollectionID, $"{theTouchpointID}");
    }
}
