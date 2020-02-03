using System;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Providers;
using Moq;
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
        /// build with null paths throws
        /// </summary>
        [Fact]
        public void BuildWithNullPathsThrows()
        {
            // arrange
            var store = MakeStrictMock<IStoreDocuments>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(null, store));
        }

        /// <summary>
        /// build with null store throws
        /// </summary>
        [Fact]
        public void BuildWithNullStoreThrows()
        {
            // arrange
            var paths = MakeStrictMock<IProvideStoragePaths>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(paths, null));
        }

        /// <summary>
        /// build meets verification
        /// </summary>
        [Fact]
        public void BuildMeetsVerification()
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
            const string touchpoint = "0000000001";
            var documentPath = new Uri("/", UriKind.Relative);

            GetMock(sut.DocumentStore)
                .Setup(x => x.GetDocument<RoutingDetail>(documentPath))
                .Returns(Task.FromResult(new RoutingDetail()));
            GetMock(sut.StoragePaths)
                .Setup(x => x.GetRoutingDetailResourcePathFor(touchpoint))
                .Returns(documentPath);

            // act
            var result = await sut.Get(touchpoint);

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
