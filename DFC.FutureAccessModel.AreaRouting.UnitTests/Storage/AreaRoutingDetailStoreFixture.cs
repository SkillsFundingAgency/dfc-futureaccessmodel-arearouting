using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Wrappers;
using Microsoft.Azure.Cosmos;
using Moq;
using System;
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
        /// build with null cosmos db provider throws
        /// </summary>
        [Fact]
        public void BuildWithNullCosmosDbProviderThrows()
        {
            // act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(null));
        }

        /// <summary>
        /// build meets verification
        /// </summary>
        [Fact]
        public void BuildMeetsVerification()
        {
            // arrange            
            var cosmosDbProvider = MakeStrictMock<IWrapCosmosDbClient>();

            // act
            var sut = MakeSUT(cosmosDbProvider);

            // assert
            GetMock(sut.CosmosDbProvider).VerifyAll();
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

            GetMock(sut.CosmosDbProvider)
                .Setup(x => x.GetAreaRoutingDetailAsync(touchpoint))
                .Returns(Task.FromResult(new RoutingDetail()));

            // act
            var result = await sut.Get(touchpoint);

            // assert
            GetMock(sut.CosmosDbProvider).VerifyAll();
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

            var sut = MakeSUT();

            GetMock(sut.CosmosDbProvider)
                .Setup(x => x.GetAllAreaRoutingsAsync())
                .Returns(Task.FromResult(touchpoints.ToList()));

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

            GetMock(sut.CosmosDbProvider)
                .Setup(x => x.AreaRoutingDetailExistsAsync(touchpoint, "not_required"))
                .Returns(Task.FromResult(false));

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

            var mockItemResponse = new Mock<ItemResponse<RoutingDetail>>();

            GetMock(sut.CosmosDbProvider)
                .Setup(x => x.AreaRoutingDetailExistsAsync(touchpoint, "not_required"))
                .Returns(Task.FromResult(true));
            GetMock(sut.CosmosDbProvider)
                .Setup(x => x.DeleteAreaRoutingDetailAsync(touchpoint, "not_required"))
                .Returns(Task.FromResult(mockItemResponse.Object));

            // act
            await sut.Delete(touchpoint);

            // assert
            GetMock(sut.CosmosDbProvider).VerifyAll();
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <returns>the system under test</returns>
        internal AreaRoutingDetailStore MakeSUT()
        {
            var cosmosDbProvider = MakeStrictMock<IWrapCosmosDbClient>();

            return MakeSUT(cosmosDbProvider);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <param name="paths">the storage paths provider</param>
        /// <param name="store">the document store</param>
        /// <returns>the system under test</returns>
        internal AreaRoutingDetailStore MakeSUT(
            IWrapCosmosDbClient cosmosDbProvider) =>
            new AreaRoutingDetailStore(cosmosDbProvider);
    }
}
