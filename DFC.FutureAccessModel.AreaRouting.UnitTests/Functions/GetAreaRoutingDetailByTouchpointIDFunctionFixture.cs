using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Adapters;
using DFC.FutureAccessModel.AreaRouting.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Functions
{
    /// <summary>
    /// the get area routing detail by touchpoint ID function fixture
    /// </summary>
    public sealed class GetAreaRoutingDetailByTouchpointIDFunctionFixture :
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

            // act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(null, adapter));
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

            // act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(factory, null));
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
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Run(null, trace, ""));
        }

        /// <summary>
        /// run with null trace throws
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task RunWithNullTraceThrows()
        {
            // arrange
            var sut = MakeSUT();
            var request = MakeStrictMock<HttpRequest>();

            // act / assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Run(request, null, ""));
        }

        /// <summary>
        /// run meets expectation
        /// </summary>
        [Fact]
        public async Task RunMeetsExpectation()
        {
            // arrange
            const string theTouchpoint = "any old touchpoint";

            var request = MakeStrictMock<HttpRequest>();
            var trace = MakeStrictMock<ILogger>();
            var scope = MakeStrictMock<IScopeLoggingContext>();
            GetMock(scope)
                .Setup(x => x.Dispose());

            var sut = MakeSUT();
            GetMock(sut.Factory)
                .Setup(x => x.BeginScopeFor(request, trace, "RunActionScope"))
                .Returns(Task.FromResult(scope));
            GetMock(sut.Adapter)
                .Setup(x => x.GetAreaRoutingDetailFor(theTouchpoint, scope))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            // act
            var result = await sut.Run(request, trace, theTouchpoint);

            // assert
            Assert.IsAssignableFrom<HttpResponseMessage>(result);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <returns>the system under test</returns>
        internal GetAreaRoutingDetailByTouchpointIDFunction MakeSUT()
        {
            var factory = MakeStrictMock<ICreateLoggingContextScopes>();
            var adapter = MakeStrictMock<IManageAreaRoutingDetails>();

            return MakeSUT(factory, adapter);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <param name="factory">the logging scope factory</param>
        /// <param name="adapter">the area routing management function adapter</param>
        /// <returns>the system under test</returns>
        internal GetAreaRoutingDetailByTouchpointIDFunction MakeSUT(
            ICreateLoggingContextScopes factory,
            IManageAreaRoutingDetails adapter) =>
                new GetAreaRoutingDetailByTouchpointIDFunction(factory, adapter);
    }
}
