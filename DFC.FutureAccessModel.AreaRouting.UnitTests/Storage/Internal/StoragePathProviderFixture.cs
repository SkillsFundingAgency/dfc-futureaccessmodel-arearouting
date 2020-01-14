using System;
using DFC.FutureAccessModel.AreaRouting.Providers;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Storage.Internal
{
    /// <summary>
    /// the storage path provider fixture
    /// </summary>
    public sealed class StoragePathProviderFixture :
        MoqTestingFixture
    {
        /// <summary>
        ///  the document store id key
        /// </summary>
        const string storeIDKey = "DocumentStoreID";

        /// <summary>
        /// the routing collection id key
        /// </summary>
        const string storeRoutingCollectionIDKey = "RoutingDetailCollectionID";

        /// <summary>
        /// the local authority collection id key
        /// </summary>
        const string storeLACollectionIDKey = "LocalAuthorityCollectionID";

        /// <summary>
        /// the (test) store name
        /// </summary>
        const string documentStoreName = "Store";

        /// <summary>
        /// the (test) area detail collection name 
        /// </summary>
        const string routingCollectionName = "Details";

        /// <summary>
        /// the (test) local authority collection name
        /// </summary>
        const string authorityCollectionName = "Authorities";

        /// <summary>
        /// the system under test supports it's service contract
        /// </summary>
        [Fact]
        public void TheSystemUnderTestSupportsItsServiceContract()
        {
            // arrange / act / assert
            Assert.IsAssignableFrom<IProvideStoragePaths>(MakeSUT());
        }

        [Fact]
        public void StoragePathProviderDocumentStoreIDKeyMeetsExpectation()
        {
            // arrange / act / assert
            Assert.Equal(storeIDKey, StoragePathProvider.DocumentStoreIDKey);
        }

        [Fact]
        public void StoragePathProviderRoutingDetailCollectionIDKeyMeetsExpectation()
        {
            // arrange / act / assert
            Assert.Equal(storeRoutingCollectionIDKey, StoragePathProvider.RoutingDetailCollectionIDKey);
        }

        [Fact]
        public void StoragePathProviderLocalAuthorityCollectionIDKeyMeetsExpectation()
        {
            // arrange / act / assert
            Assert.Equal(storeLACollectionIDKey, StoragePathProvider.LocalAuthorityCollectionIDKey);
        }

        /// <summary>
        /// new storage path provider build fails with null settings
        /// </summary>
        [Fact]
        public void NewStoragePathProviderBuildFailsWithNullSettings()
        {
            // arrange / act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(null));
        }

        /// <summary>
        /// new storage path provider build meets verification
        /// </summary>
        [Fact]
        public void NewStoragePathProviderBuildMeetsVerification()
        {
            // arrange / act
            var sut = MakeSUT();

            // assert
            GetMock(sut.Settings).VerifyAll();
        }

        /// <summary>
        /// get routing details resource path for the touchpoint id meets expectation
        /// </summary>
        [Fact]
        public void GetRoutingDetailResourcePathFortheTouchpointIDMeetsExpectation()
        {
            // arrange
            var sut = MakeSUT();

            // act
            var result = sut.GetRoutingDetailResourcePathFor("0000000001");

            // assert
            Assert.Equal("dbs/Store/colls/Details/docs/0000000001", result.OriginalString);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <returns>the system under test</returns>
        internal StoragePathProvider MakeSUT()
        {
            var settings = MakeStrictMock<IProvideApplicationSettings>();
            GetMock(settings)
                .Setup(x => x.GetVariable(StoragePathProvider.DocumentStoreIDKey))
                .Returns(documentStoreName);
            GetMock(settings)
                .Setup(x => x.GetVariable(StoragePathProvider.RoutingDetailCollectionIDKey))
                .Returns(routingCollectionName);
            GetMock(settings)
                .Setup(x => x.GetVariable(StoragePathProvider.LocalAuthorityCollectionIDKey))
                .Returns(authorityCollectionName);

            return MakeSUT(settings);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <param name="paths">the storage paths provider</param>
        /// <param name="store">the document store</param>
        /// <returns>the system under test</returns>
        internal StoragePathProvider MakeSUT(
            IProvideApplicationSettings settings) =>
            new StoragePathProvider(settings);
    }
}
