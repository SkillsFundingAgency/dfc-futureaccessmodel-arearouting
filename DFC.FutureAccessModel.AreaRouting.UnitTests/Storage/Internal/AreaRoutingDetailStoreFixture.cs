using System;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Models;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Storage.Internal
{
    /// <summary>
    /// the area routing detail storage provider fixture
    /// </summary>
    public sealed class AreaRoutingDetailStoreFixture :
        MoqTestingFixture
    {
        /// <summary>
        /// the system under test supports it's service contract
        /// </summary>
        [Fact]
        public void TheSystemUnderTestSupportsItsServiceContract()
        {
            // arrange / act / assert
            Assert.IsAssignableFrom<IStoreAreaRoutingDetails>(MakeSUT());
        }

        /// <summary>
        /// new area routing detail store build fails with null paths
        /// </summary>
        [Fact]
        public void NewAreaRoutingDetailStoreBuildFailsWithNullPaths()
        {
            // arrange
            var store = MakeStrictMock<IStoreDocuments>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(null, store));
        }

        /// <summary>
        /// new area routing detail store build fails with null store
        /// </summary>
        [Fact]
        public void NewAreaRoutingDetailStoreBuildFailsWithNullStore()
        {
            // arrange
            var paths = MakeStrictMock<IProvideStoragePaths>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(paths, null));
        }

        /// <summary>
        /// new area routing detail store build meets verification
        /// </summary>
        [Fact]
        public void NewAreaRoutingDetailStoreBuildMeetsVerification()
        {
            // arrange
            var paths = MakeStrictMock<IProvideStoragePaths>();
            var store = MakeStrictMock<IStoreDocuments>();

            // act
            var sut = MakeSUT(paths, store);

            // assert
            GetMock(sut.DocumentStore).VerifyAll();
            GetMock(sut.StoragePaths).VerifyAll();
        }

        /// <summary>
        /// get routing detail for the touchpoint id meets verification
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task GetRoutingDetailForTheTouchpointIDMeetsVerification()
        {
            // arrange
            var sut = MakeSUT();

            // act
            var result = await sut.GetAreaRoutingDetailFor("0000000001");

            // assert
            GetMock(sut.DocumentStore).VerifyAll();
            GetMock(sut.StoragePaths).VerifyAll();
            Assert.IsAssignableFrom<IRoutingDetail>(result);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <returns>the system under test</returns>
        internal AreaRoutingDetailStore MakeSUT()
        {
            var paths = MakeStrictMock<IProvideStoragePaths>();
            var store = MakeStrictMock<IStoreDocuments>();

            return MakeSUT(paths, store);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <param name="paths">the storage paths provider</param>
        /// <param name="store">the document store</param>
        /// <returns>the system under test</returns>
        internal AreaRoutingDetailStore MakeSUT(
            IProvideStoragePaths paths,
            IStoreDocuments store) =>
            new AreaRoutingDetailStore(paths, store);
    }
}
