using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                .Setup(x => x.GetDocument<RoutingDetail>(documentPath, "not_required"))
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
        /// get all meets expectation
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task GetAllMeetsExpectation()
        {
            // arrange
            RoutingDetail[] touchpoints = {
                new RoutingDetail { TouchpointID = "000101" },
                new RoutingDetail { TouchpointID = "000102" },
                new RoutingDetail { TouchpointID = "000103" }
            };

            var collectionPath = new Uri("dbs/cosmicThingy/colls/collectionThingy", UriKind.Relative);

            var sut = MakeSUT();

            GetMock(sut.StoragePaths)
                .SetupGet(x => x.RoutingDetailCollection)
                .Returns(collectionPath);
            GetMock(sut.DocumentStore)
                .Setup(x => x.CreateDocumentQuery<RoutingDetail>(collectionPath, "select * from c"))
                .Returns(Task.FromResult<IReadOnlyCollection<RoutingDetail>>(touchpoints));

            // act
            var result = await sut.GetAllIDs();

            // assert
            Assert.Equal(3, result.Count);
            Assert.Contains(result, x => touchpoints.Any(y => y.TouchpointID == x));
        }

        /// <summary>
        /// delete routing detail with null touchpoint throws
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task DeleteRoutingDetailWithNullTouchpointThrows()
        {
            // arrange
            var sut = MakeSUT();

            // act / assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Delete(null));
        }

        /// <summary>
        /// delete routing detail with invalid touchpoint throws
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task DeleteRoutingDetailWithInvalidTouchpointThrows()
        {
            // arrange
            var sut = MakeSUT();
            const string touchpoint = "0000000001";
            var documentPath = new Uri("/", UriKind.Relative);

            GetMock(sut.DocumentStore)
                .Setup(x => x.DocumentExists<RoutingDetail>(documentPath, "not_required"))
                .Returns(Task.FromResult(false));
            GetMock(sut.StoragePaths)
                .Setup(x => x.GetRoutingDetailResourcePathFor(touchpoint))
                .Returns(documentPath);

            // act / assert
            await Assert.ThrowsAsync<NoContentException>(() => sut.Delete(touchpoint));
        }

        /// <summary>
        /// delete routing detail with valid touchpoint meets verificaiton
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task DeleteRoutingDetailWithValidTouchpointMeetsVerification()
        {
            // arrange
            var sut = MakeSUT();
            const string touchpoint = "0000000001";
            var documentPath = new Uri("/", UriKind.Relative);

            GetMock(sut.DocumentStore)
                .Setup(x => x.DocumentExists<RoutingDetail>(documentPath, "not_required"))
                .Returns(Task.FromResult(true));
            GetMock(sut.DocumentStore)
                .Setup(x => x.DeleteDocument(documentPath, "not_required"))
                .Returns(Task.CompletedTask);
            GetMock(sut.StoragePaths)
                .Setup(x => x.GetRoutingDetailResourcePathFor(touchpoint))
                .Returns(documentPath);

            // act
            await sut.Delete(touchpoint);

            // assert
            GetMock(sut.DocumentStore).VerifyAll();
            GetMock(sut.StoragePaths).VerifyAll();
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
