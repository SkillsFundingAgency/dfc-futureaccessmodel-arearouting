using System;
using Microsoft.Azure.Documents.Client;

namespace DFC.FutureAccessModel.AreaRouting.Storage.Internal
{
    internal static class StoragePath
    {
        public const string DocumentStoreIDKey = "DocumentStoreID";
        public const string RoutingDetailCollectionIDKey = "RoutingDetailCollectionID";
        public const string LocalAuthorityCollectionIDKey = "LocalAuthorityCollectionID";

        public static Uri RoutingDetailCollection { get; }
        public static Uri LocalAuthorityCollection { get; }

        // TODO: find out how these environment variables are created
        public static string DocumentStoreID { get; }
        public static string RoutingDetailCollectionID { get; }
        public static string LocalAuthorityCollectionID { get; }

        static StoragePath()
        {
            DocumentStoreID = Environment.GetEnvironmentVariable(DocumentStoreIDKey);
            RoutingDetailCollectionID = Environment.GetEnvironmentVariable(RoutingDetailCollectionIDKey);
            LocalAuthorityCollectionID = Environment.GetEnvironmentVariable(LocalAuthorityCollectionIDKey);

            RoutingDetailCollection = UriFactory.CreateDocumentCollectionUri(DocumentStoreID, RoutingDetailCollectionID);
            LocalAuthorityCollection = UriFactory.CreateDocumentCollectionUri(DocumentStoreID, LocalAuthorityCollectionID);
        }

        internal static Uri GetRoutingDetailResourcePathFor(string theTouchpointID) =>
            UriFactory.CreateDocumentUri(DocumentStoreID, RoutingDetailCollectionID, $"{theTouchpointID}");
    }
}
