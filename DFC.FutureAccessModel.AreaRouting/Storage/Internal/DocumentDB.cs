using System;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using Microsoft.Azure.Documents.Client;

namespace DFC.FutureAccessModel.AreaRouting.Storage.Internal
{
    internal static class DocumentDB
    {
        public const string DocumentStoreConnectionDetailKey = "AreaRoutingStorageConnectionDetail";
        public const string ValueSeparatorKey = "=";
        public const string ValueTerminatorKey = ";";

        public static string EndpointAddress { get; }
        public static string AccountKey { get; }

        static DocumentDB()
        {
            var connectionString = Environment.GetEnvironmentVariable(DocumentStoreConnectionDetailKey, EnvironmentVariableTarget.Process);

            It.IsEmpty(connectionString)
                .AsGuard<ArgumentNullException>(nameof(connectionString));

            var firstStart = connectionString.IndexOf(ValueSeparatorKey, StringComparison.InvariantCultureIgnoreCase) + 1;
            var firstStop = connectionString.IndexOf(ValueTerminatorKey, StringComparison.InvariantCultureIgnoreCase) - 1;
            var lastStart = connectionString.LastIndexOf(ValueSeparatorKey, StringComparison.InvariantCultureIgnoreCase) + 1;
            var lastStop = connectionString.LastIndexOf(ValueTerminatorKey, StringComparison.InvariantCultureIgnoreCase) - 1;

            EndpointAddress = connectionString.Substring(firstStart, firstStop);

            It.IsEmpty(EndpointAddress)
                .AsGuard<ArgumentNullException>(nameof(EndpointAddress));

            AccountKey = connectionString.Substring(lastStart, lastStop);

            It.IsEmpty(AccountKey)
                .AsGuard<ArgumentNullException>(nameof(AccountKey));

            Client = new DocumentClient(new Uri(EndpointAddress), AccountKey);
        }

        internal static DocumentClient Client { get; }
    }
}
