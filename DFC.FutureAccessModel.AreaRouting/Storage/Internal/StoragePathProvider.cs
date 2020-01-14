using System;
using DFC.FutureAccessModel.AreaRouting.Helpers;
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
        /// <summary>
        /// the document store id (variable) key
        /// </summary>
        public const string DocumentStoreIDKey = "DocumentStoreID";

        /// <summary>
        /// the routing detail collections (variable) key
        /// </summary>
        public const string RoutingDetailCollectionIDKey = "RoutingDetailCollectionID";

        /// <summary>
        /// the loca lauthority collection (variable) key
        /// </summary>
        public const string LocalAuthorityCollectionIDKey = "LocalAuthorityCollectionID";

        /// <summary>
        /// the document store id (variable value)
        /// </summary>
        public string DocumentStoreID { get; }

        /// <summary>
        /// the routing detail collection id (variable value)
        /// </summary>
        public string RoutingDetailCollectionID { get; }

        /// <summary>
        /// the local authority collection id (variable value)
        /// </summary>
        public string LocalAuthorityCollectionID { get; }

        /// <summary>
        /// the routing detail coolection (path)
        /// </summary>
        public Uri RoutingDetailCollection { get; }

        /// <summary>
        /// the local authority collection (path)
        /// </summary>
        public Uri LocalAuthorityCollection { get; }

        /// <summary>
        /// the applicaton settings provider
        /// </summary>
        public IProvideApplicationSettings Settings { get; }

        /// <summary>
        /// initialises the storage path provider
        /// </summary>
        public StoragePathProvider(IProvideApplicationSettings settings)
        {
            It.IsNull(settings)
                .AsGuard<ArgumentNullException>(nameof(settings));

            Settings = settings;

            DocumentStoreID = Settings.GetVariable(DocumentStoreIDKey);
            RoutingDetailCollectionID = Settings.GetVariable(RoutingDetailCollectionIDKey);
            LocalAuthorityCollectionID = Settings.GetVariable(LocalAuthorityCollectionIDKey);

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
