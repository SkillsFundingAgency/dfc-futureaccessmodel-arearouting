using DFC.FutureAccessModel.AreaRouting.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Wrappers.Internal
{
    public class CosmosDbWrapperFixture :
        MoqTestingFixture
    {
        private readonly Mock<CosmosClient> _mockCosmosClient;
        private readonly Mock<IOptions<ConfigurationSettings>> _mockConfigSettings;
        private readonly Mock<ILogger<CosmosDbWrapper>> _mockLogger;

        const string DocumentStoreID = "database";
        const string RoutingDetailCollectionID = "area";
        const string LocalAuthorityCollectionID = "localauthority";

        public CosmosDbWrapperFixture()
        {
            _mockCosmosClient = new Mock<CosmosClient>();
            _mockConfigSettings = new Mock<IOptions<ConfigurationSettings>>();
            _mockLogger = new Mock<ILogger<CosmosDbWrapper>>();

            var configSettings = new ConfigurationSettings
            {
                DocumentStoreID = DocumentStoreID,
                DocumentStoreAccountKey = "sdafsdkfjsdalfkjasdfkld",
                DocumentStoreEndpointAddress = "asdflksjadfksdjfklsd",
                RoutingDetailCollectionID = RoutingDetailCollectionID,
                LocalAuthorityCollectionID = LocalAuthorityCollectionID,
                MSDEPLOY_RENAME_LOCKED_FILES = "1",
                WEBSITE_RUN_FROM_PACKAGE = "abc",
            };

            var mockContainer = new Mock<Container>();
            var mockItemResponse = new Mock<ItemResponse<RoutingDetail>>();

            _mockCosmosClient.Setup(x => x.GetContainer(configSettings.DocumentStoreID, configSettings.RoutingDetailCollectionID)).Returns(mockContainer.Object);
            mockContainer.Setup(x => x.CreateItemAsync(It.IsAny<RoutingDetail>(), new PartitionKey("not_required"), null, It.IsAny<CancellationToken>())).ReturnsAsync(mockItemResponse.Object);

            _mockConfigSettings.SetupGet(c => c.Value).Returns(configSettings);
        }

        /// <summary>
        /// the system under test supports it's service contract
        /// </summary>
        [Fact]
        public void TheSystemUnderTestSupportsItsServiceContract()
        {
            // arrange / act / assert
            Assert.IsAssignableFrom<IWrapCosmosDbClient>(MakeSUT(_mockCosmosClient.Object, _mockConfigSettings.Object, _mockLogger.Object));
        }

        /// <summary>
        /// build with null client throws
        /// </summary>
        [Fact]
        public void BuildWithNullClientThrows()
        {
            // arrange / act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(null, _mockConfigSettings.Object, _mockLogger.Object));
        }

        /// <summary>
        /// build with null config settings throws
        /// </summary>
        [Fact]
        public void BuildWithNullConfigSettingsThrows()
        {
            var cosmosClient = new Mock<CosmosClient>();
            var logger = new Mock<ILogger<CosmosDbWrapper>>();

            // arrange / act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(cosmosClient.Object, null, logger.Object));
        }

        /// <summary>
        /// build with null logger throws
        /// </summary>
        [Fact]
        public void BuildWithNullLoggerThrows()
        {
            // arrange / act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(_mockCosmosClient.Object, _mockConfigSettings.Object, null));
        }

        /// <summary>
        /// create area routing detail (async) meets expectation
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task CreateAreaRoutingDetailAsyncMeetsExpectation()
        {
            // arrange
            const string keyValue = "0000000209";
            const string partitionKey = "not_required";

            var sut = MakeSUT(_mockCosmosClient.Object, _mockConfigSettings.Object, _mockLogger.Object);

            var routingDetail = new RoutingDetail
            {
                TouchpointID = keyValue,
                Area = "Yorkshire and the Humber",
                TelephoneNumber = "0191 500 8736",
                SMSNumber = "07766 413219",
                EmailAddress = "digital.first.careers+SIT_YRKHUM@gmail.com"
            };


            // act
            var result = await sut.CreateAreaRoutingDetailAsync(routingDetail, partitionKey);

            // assert
            Assert.IsAssignableFrom<ItemResponse<RoutingDetail>>(result);
        }

        /// <summary>
        /// CreateAreaRoutingDetailAsync method returns exception when input parameter RoutingDetail is null
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task CreateAreaRoutingDetailAsyncThrowsExceptionWhenRoutingDetailObjectIsNull()
        {
            // arrange
            const string keyValue = "0000000209";
            const string partitionKey = "not_required";

            var sut = MakeSUT(_mockCosmosClient.Object, _mockConfigSettings.Object, _mockLogger.Object);

            var routingDetail = new RoutingDetail
            {
                TouchpointID = keyValue,
                Area = "Yorkshire and the Humber",
                TelephoneNumber = "0191 500 8736",
                SMSNumber = "07766 413219",
                EmailAddress = "digital.first.careers+SIT_YRKHUM@gmail.com"
            };

            // act / assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.CreateAreaRoutingDetailAsync(null, partitionKey));
        }

        /// <summary>
        /// make a 'system under test'
        /// </summary>
        /// <returns>a system under test</returns>
        internal CosmosDbWrapper MakeSUT(CosmosClient client, IOptions<ConfigurationSettings> configOptions, ILogger<CosmosDbWrapper> logger) =>
            new CosmosDbWrapper(client, configOptions, logger);
    }
}
