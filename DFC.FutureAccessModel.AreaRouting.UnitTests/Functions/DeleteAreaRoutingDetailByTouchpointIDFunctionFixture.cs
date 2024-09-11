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
    /// the get area routing detail by touchpoint ID function fixture
    /// </summary>
    public sealed class DeleteAreaRoutingDetailByTouchpointIDFunctionFixture :
        MoqTestingFixture
    {
        /// <summary>
        /// run with null factory throws
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public void RunWithNullFactoryThrows()
        {
            // arrange
            var adapter = MakeStrictMock<IManageAreaRoutingDetails>();
            var logger = MakeStrictMock<ILogger<DeleteAreaRoutingDetailByTouchpointIDFunction>>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(null, adapter, logger));
        }

        /// <summary>
        /// run with null adapter throws
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public void RunWithNullAdapterThrows()
        {
            // arrange
            var factory = MakeStrictMock<ICreateLoggingContextScopes>();
            var logger = MakeStrictMock<ILogger<DeleteAreaRoutingDetailByTouchpointIDFunction>>();

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

            // act / assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Run(null, ""));
        }

        /// <summary>
        /// run meets expectation
        /// </summary>

        [Fact]
        public async Task RunMeetsExpectation()
        {
            // Arrange
            const string theTouchpoint = "any old touchpoint";
            var request = new DefaultHttpContext().Request;

            var logger = new Mock<ILogger<DeleteAreaRoutingDetailByTouchpointIDFunction>>();
            var scope = new Mock<IScopeLoggingContext>();
            var factory = new Mock<ICreateLoggingContextScopes>();
            var adapter = new Mock<IManageAreaRoutingDetails>();

            var sut = new DeleteAreaRoutingDetailByTouchpointIDFunction(factory.Object, adapter.Object, logger.Object);

            scope.Setup(x => x.Dispose());
            GetMock(sut.Factory)
                .Setup(x => x.BeginScopeFor(request, logger.Object, "RunActionScope"))
                .Returns(Task.FromResult(scope.Object));
            GetMock(sut.Adapter)
                .Setup(x => x.DeleteAreaRoutingDetailUsing(theTouchpoint, scope.Object))
                .Returns(Task.FromResult<IActionResult>(new OkResult()));

            // act
            var result = await sut.Run(request, theTouchpoint);

            // assert
            Assert.IsAssignableFrom<OkResult>(result);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <returns>the system under test</returns>
        internal DeleteAreaRoutingDetailByTouchpointIDFunction MakeSUT()
        {
            var factory = MakeStrictMock<ICreateLoggingContextScopes>();
            var adapter = MakeStrictMock<IManageAreaRoutingDetails>();
            var logger = MakeStrictMock<ILogger<DeleteAreaRoutingDetailByTouchpointIDFunction>>();

            return MakeSUT(factory, adapter, logger);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <param name="factory">the logging scope factory</param>
        /// <param name="adapter">the area routing management function adapter</param>
        /// <returns>the system under test</returns>
        internal DeleteAreaRoutingDetailByTouchpointIDFunction MakeSUT(
            ICreateLoggingContextScopes factory,
            IManageAreaRoutingDetails adapter,
            ILogger<DeleteAreaRoutingDetailByTouchpointIDFunction> logger) =>
                new DeleteAreaRoutingDetailByTouchpointIDFunction(factory, adapter, logger);
    }
}
