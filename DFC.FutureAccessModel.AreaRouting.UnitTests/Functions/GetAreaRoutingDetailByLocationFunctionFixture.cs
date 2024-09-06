using DFC.FutureAccessModel.AreaRouting.Adapters;
using DFC.FutureAccessModel.AreaRouting.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

            // act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(null, adapter));
        }

        /// <summary>
        /// run with null adapter throws
        /// </summary>
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
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Run(null, trace));
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
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Run(request, null));
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

            var request = MakeStrictMock<HttpRequest>();

            GetMock(request)
                .Setup(x => x.Query.ContainsKey(locationKey))
                .Returns(true);
            GetMock(request)
                .Setup(x => x.Query[locationKey])
                .Returns(location);

            var trace = MakeStrictMock<ILogger>();

            var scope = MakeStrictMock<IScopeLoggingContext>();
            GetMock(scope)
                .Setup(x => x.Dispose());

            var sut = MakeSUT();
            GetMock(sut.Factory)
                .Setup(x => x.BeginScopeFor(request, trace, "RunActionScope"))
                .Returns(Task.FromResult(scope));
            GetMock(sut.Adapter)
                .Setup(x => x.GetAreaRoutingDetailBy(location, scope))
                .Returns(Task.FromResult<IActionResult>(new OkResult()));

            // act
            var result = await sut.Run(request, trace);

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

            return MakeSUT(factory, adapter);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <param name="factory">the logging scope factory</param>
        /// <param name="adapter">the area routing management function adapter</param>
        /// <returns>the system under test</returns>
        internal GetAreaRoutingDetailByLocationFunction MakeSUT(
            ICreateLoggingContextScopes factory,
            IManageAreaRoutingDetails adapter) =>
                new GetAreaRoutingDetailByLocationFunction(factory, adapter);
    }
}
