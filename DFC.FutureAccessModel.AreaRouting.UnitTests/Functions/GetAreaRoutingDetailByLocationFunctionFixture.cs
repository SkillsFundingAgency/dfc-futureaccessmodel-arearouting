using DFC.FutureAccessModel.AreaRouting.Adapters;
using DFC.FutureAccessModel.AreaRouting.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Functions
{
    /// <summary>
    /// the get area routing detail by location function fixture
    /// </summary>
    public sealed class GetAreaRoutingDetailByLocationFunctionFixture :
        MoqTestingFixture
    {
        /// <summary>
        /// run with null factory throws
        /// </summary>
        [Fact]
        public void RunWithNullFactoryThrows()
        {
            // arrange
            var adapter = MakeStrictMock<IManageAreaRoutingDetails>();
            var logger = MakeStrictMock<ILogger<GetAreaRoutingDetailByLocationFunction>>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(null, adapter, logger));
        }

        /// <summary>
        /// run with null adapter throws
        /// </summary>
        [Fact]
        public void RunWithNullAdapterThrows()
        {
            // arrange
            var factory = MakeStrictMock<ICreateLoggingContextScopes>();
            var logger = MakeStrictMock<ILogger<GetAreaRoutingDetailByLocationFunction>>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(factory, null, logger));
        }

        /// <summary>
        /// run with null request throws
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task RunWithNullRequestThrows()
        {
            // arrange
            var sut = MakeSUT();
            var trace = MakeStrictMock<ILogger>();

            // act / assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Run(null));
        }


        /// <summary>
        /// run meets expectation
        /// </summary>
        [Fact]
        public async Task RunMeetsExpectation()
        {
            // arrange
            const string location = "any old location";
            const string locationKey = "location";

            var request = new Mock<HttpRequest>();
            var logger = new Mock<ILogger<GetAreaRoutingDetailByLocationFunction>>();
            var factory = new Mock<ICreateLoggingContextScopes>();
            var adapter = new Mock<IManageAreaRoutingDetails>();
            var scope = new Mock<IScopeLoggingContext>();

            var sut = new GetAreaRoutingDetailByLocationFunction(factory.Object, adapter.Object, logger.Object);

            scope.Setup(x => x.Dispose());
            GetMock(request.Object)
                .Setup(x => x.Query.ContainsKey(locationKey))
                .Returns(true);
            GetMock(request.Object)
                .Setup(x => x.Query[locationKey])
                .Returns(location);
            GetMock(sut.Factory)
                .Setup(x => x.BeginScopeFor(request.Object, logger.Object, "RunActionScope"))
                .Returns(Task.FromResult(scope.Object));
            GetMock(sut.Adapter)
                .Setup(x => x.GetAreaRoutingDetailBy(location, scope.Object))
                .Returns(Task.FromResult<IActionResult>(new OkResult()));

            // act
            var result = await sut.Run(request.Object);

            // assert
            Assert.IsAssignableFrom<OkResult>(result);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <returns>the system under test</returns>
        internal GetAreaRoutingDetailByLocationFunction MakeSUT()
        {
            var factory = MakeStrictMock<ICreateLoggingContextScopes>();
            var adapter = MakeStrictMock<IManageAreaRoutingDetails>();
            var logger = MakeStrictMock<ILogger<GetAreaRoutingDetailByLocationFunction>>();

            return MakeSUT(factory, adapter, logger);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <param name="factory">the logging scope factory</param>
        /// <param name="adapter">the area routing management function adapter</param>
        /// <returns>the system under test</returns>
        internal GetAreaRoutingDetailByLocationFunction MakeSUT(
            ICreateLoggingContextScopes factory,
            IManageAreaRoutingDetails adapter,
            ILogger<GetAreaRoutingDetailByLocationFunction> logger) =>
                new GetAreaRoutingDetailByLocationFunction(factory, adapter, logger);
    }
}
