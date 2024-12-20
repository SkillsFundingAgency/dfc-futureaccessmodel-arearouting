using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.FutureAccessModel.AreaRouting.Wrappers.Internal
{
    public class CosmosDbWrapper : IWrapCosmosDbClient
    {
        private readonly Container _areaRoutingContainer;
        private readonly Container _localAuthorityContainer;
        private readonly ILogger<CosmosDbWrapper> _logger;

        public CosmosDbWrapper(CosmosClient cosmosClient,
            IOptions<ConfigurationSettings> configOptions,
            ILogger<CosmosDbWrapper> logger)
        {
            It.IsNull(cosmosClient)
                .AsGuard<ArgumentNullException>(nameof(cosmosClient));

            It.IsNull(configOptions)
                .AsGuard<ArgumentNullException>(nameof(configOptions));

            It.IsNull(logger)
                .AsGuard<ArgumentNullException>(nameof(logger));

            var config = configOptions.Value;

            _areaRoutingContainer = GetContainer(cosmosClient, config.DocumentStoreID, config.RoutingDetailCollectionID);
            _localAuthorityContainer = GetContainer(cosmosClient, config.DocumentStoreID, config.LocalAuthorityCollectionID);
            _logger = logger;
        }

        private static Container GetContainer(CosmosClient cosmosClient, string databaseId, string collectionId)
            => cosmosClient.GetContainer(databaseId, collectionId);

        public async Task<List<RoutingDetail>> GetAllAreaRoutingsAsync()
        {
            _logger.LogInformation("Retrieving all area routine Ids");

            try
            {
                var areaRoutings = new List<RoutingDetail>();
                var query = _areaRoutingContainer.GetItemLinqQueryable<RoutingDetail>()
                    .ToFeedIterator();

                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    areaRoutings.AddRange(response);
                }

                _logger.LogInformation("Retrieved {Count} Area Routing(s).", areaRoutings.Count);

                return areaRoutings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Area Routing(s). Message: {Message}", ex.Message);
                return null;
            }
        }

        public async Task<RoutingDetail> GetAreaRoutingDetailAsync(string theTouchpoint)
        {
            _logger.LogInformation("Retrieving routing detail for the touchpoint: {TheTouchpoint}.", theTouchpoint);

            try
            {
                var query = _areaRoutingContainer.GetItemLinqQueryable<RoutingDetail>()
                    .Where(ar => ar.TouchpointID == theTouchpoint)
                    .ToFeedIterator();

                var response = await query.ReadNextAsync();
                if (response.Any())
                {
                    _logger.LogInformation("Routing detail retrieved successfully for the touchpoint: {TheTouchpoint}.", theTouchpoint);
                    return response.FirstOrDefault();
                }

                _logger.LogWarning("Routing detail not found for the touchpoint: {TheTouchpoint}.", theTouchpoint);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving routing detail for the touchpoint: {TheTouchpoint}. Message: {Message}", theTouchpoint, ex.Message);
                return null;
            }
        }

        public async Task<bool> AreaRoutingDetailExistsAsync(string id, string partitionKey)
        {
            try
            {
                var response = await _areaRoutingContainer.ReadItemAsync<RoutingDetail>(id, new PartitionKey(partitionKey));
                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        public async Task<LocalAuthority> GetLocalAuthorityAsync(string theAdminDistrict)
        {
            _logger.LogInformation("Retrieving local authority for the district: {TheAdminDistrict}.", theAdminDistrict);

            try
            {
                var query = _localAuthorityContainer.GetItemLinqQueryable<LocalAuthority>()
                    .Where(la => la.LADCode == theAdminDistrict)
                    .ToFeedIterator();

                var response = await query.ReadNextAsync();
                if (response.Any())
                {
                    _logger.LogInformation("Local authority retrieved successfully for the district: {TheAdminDistrict}.", theAdminDistrict);
                    return response.FirstOrDefault();
                }

                _logger.LogWarning("Local authority not found for the district: {TheAdminDistrict}.", theAdminDistrict);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving local authority for the district: {TheAdminDistrict}. Message: {Message}", theAdminDistrict, ex.Message);
                return null;
            }
        }

        public async Task<ItemResponse<RoutingDetail>> CreateAreaRoutingDetailAsync(RoutingDetail routingDetail, string partitionKey)
        {
            if (routingDetail == null)
            {
                _logger.LogError("RoutingDetail object is null. Creation aborted.");
                throw new ArgumentNullException(nameof(routingDetail), "Routing detail cannot be null.");
            }

            _logger.LogInformation("Creating area routing detail with touchpoint: {Touchpoint}", routingDetail.TouchpointID);

            try
            {
                var response = await _areaRoutingContainer.CreateItemAsync(routingDetail, new PartitionKey(partitionKey));
                _logger.LogInformation("Successfully created area routing detail with touchpoint: {Touchpoint}", routingDetail.TouchpointID);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create area routing detail with touchpoint: {Touchpoint}", routingDetail.TouchpointID);
                return null;
            }
        }

        public async Task<ItemResponse<RoutingDetail>> DeleteAreaRoutingDetailAsync(string theTouchpoint, string partitionKey)
        {
            if (string.IsNullOrEmpty(theTouchpoint))
            {
                _logger.LogError("Touchpoint object is either null or empty. Creation aborted.");
                throw new ArgumentNullException(nameof(theTouchpoint), "Touchpoint cannot be null or empty.");
            }

            _logger.LogInformation("Deleting area routing detail with touchpoint: {Touchpoint}", theTouchpoint);

            try
            {
                var response = await _areaRoutingContainer.DeleteItemAsync<RoutingDetail>(theTouchpoint, new PartitionKey(partitionKey));
                _logger.LogInformation("Successfully deleted area routing detail with touchpoint: {Touchpoint}", theTouchpoint);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete area routing detail with touchpoint: {Touchpoint}", theTouchpoint);
                return null;
            }
        }

    }
}