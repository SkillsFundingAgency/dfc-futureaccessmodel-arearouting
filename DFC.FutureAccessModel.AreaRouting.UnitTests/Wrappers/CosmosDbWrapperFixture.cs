using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Wrappers.Internal
{
    public class CosmosDbWrapperFixture :
        MoqTestingFixture
    {
        /// <summary>
        /// the system under test supports it's service contract
        /// </summary>
        [Fact]
        public void TheSystemUnderTestSupportsItsServiceContract()
        {
            // arrange
            var cosmosClient = new Mock<CosmosClient>();
            var mockConfigSettings = new Mock<IOptions<Models.ConfigurationSettings>>();
            var logger = new Mock<ILogger<CosmosDbWrapper>>();

            var configSettings = new Models.ConfigurationSettings
            {
                DocumentStoreID = "database",
                DocumentStoreAccountKey = "sdafsdkfjsdalfkjasdfkld",
                DocumentStoreEndpointAddress = "asdflksjadfksdjfklsd",
                RoutingDetailCollectionID = "area",
                LocalAuthorityCollectionID = "localauthority",
                MSDEPLOY_RENAME_LOCKED_FILES = "1",
                WEBSITE_RUN_FROM_PACKAGE = "abc",
            };

            mockConfigSettings.SetupGet(c => c.Value).Returns(configSettings);


            //  act / assert
            Assert.IsAssignableFrom<IWrapCosmosDbClient>(MakeSUT(cosmosClient.Object, mockConfigSettings.Object, logger.Object));
        }

        /// <summary>
        /// build with null client throws
        /// </summary>
        [Fact]
        public void BuildWithNullClientThrows()
        {
            var mockConfigSettings = new Mock<IOptions<Models.ConfigurationSettings>>();
            var logger = new Mock<ILogger<CosmosDbWrapper>>();
            var configSettings = new Models.ConfigurationSettings
            {
                DocumentStoreID = "database",
                DocumentStoreAccountKey = "sdafsdkfjsdalfkjasdfkld",
                DocumentStoreEndpointAddress = "asdflksjadfksdjfklsd",
                RoutingDetailCollectionID = "area",
                LocalAuthorityCollectionID = "localauthority",
                MSDEPLOY_RENAME_LOCKED_FILES = "1",
                WEBSITE_RUN_FROM_PACKAGE = "abc",
            };

            mockConfigSettings.SetupGet(c => c.Value).Returns(configSettings);

            // arrange / act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(null, mockConfigSettings.Object, logger.Object));
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
            // arrange
            var cosmosClient = new Mock<CosmosClient>();
            var mockConfigSettings = new Mock<IOptions<Models.ConfigurationSettings>>();

            var configSettings = new Models.ConfigurationSettings
            {
                DocumentStoreID = "database",
                DocumentStoreAccountKey = "sdafsdkfjsdalfkjasdfkld",
                DocumentStoreEndpointAddress = "asdflksjadfksdjfklsd",
                RoutingDetailCollectionID = "area",
                LocalAuthorityCollectionID = "localauthority",
                MSDEPLOY_RENAME_LOCKED_FILES = "1",
                WEBSITE_RUN_FROM_PACKAGE = "abc",
            };

            mockConfigSettings.SetupGet(c => c.Value).Returns(configSettings);


            //  act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(cosmosClient.Object, mockConfigSettings.Object, null));
        }

        /// <summary>
        /// make a 'system under test'
        /// </summary>
        /// <returns>a system under test</returns>
        internal CosmosDbWrapper MakeSUT(CosmosClient client, IOptions<Models.ConfigurationSettings> configOptions, ILogger<CosmosDbWrapper> logger) =>
            new CosmosDbWrapper(client, configOptions, logger);
    }
}
