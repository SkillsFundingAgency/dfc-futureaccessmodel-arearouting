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
    public sealed class DeleteAreaRoutingDetailByTouchpointIDFunctionFixture :
        MoqTestingFixture
    {
        /// <summary>
        /// run with null request throws
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task RunWithNullRequestThrows()
        {
            // arrange
            var trace = MakeStrictMock<ILogger>();
            var factory = MakeStrictMock<ICreateLoggingContextScopes>();
            var adapter = MakeStrictMock<IManageAreaRoutingDetails>();

            // act / assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => DeleteAreaRoutingDetailByTouchpointIDFunction.Run(null, trace, "", factory, adapter));
        }

        /// <summary>
        /// run with null trace throws
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task RunWithNullTraceThrows()
        {
            // arrange
            var request = MakeStrictMock<HttpRequest>();
            var factory = MakeStrictMock<ICreateLoggingContextScopes>();
            var adapter = MakeStrictMock<IManageAreaRoutingDetails>();

            // act / assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => DeleteAreaRoutingDetailByTouchpointIDFunction.Run(request, null, "", factory, adapter));
        }

        /// <summary>
        /// run with null factory throws
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task RunWithNullFactoryThrows()
        {
            // arrange
            var request = MakeStrictMock<HttpRequest>();
            var trace = MakeStrictMock<ILogger>();
            var adapter = MakeStrictMock<IManageAreaRoutingDetails>();

            // act / assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => DeleteAreaRoutingDetailByTouchpointIDFunction.Run(request, trace, "", null, adapter));
        }

        /// <summary>
        /// run with null adapter throws
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task RunWithNullAdapterThrows()
        {
            // arrange
            var request = MakeStrictMock<HttpRequest>();
            var trace = MakeStrictMock<ILogger>();
            var factory = MakeStrictMock<ICreateLoggingContextScopes>();

            // act / assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => DeleteAreaRoutingDetailByTouchpointIDFunction.Run(request, trace, "", factory, null));
        }

        /// <summary>
        /// run meets expectation
        /// </summary>
        [Fact]
        public async Task RunMeetsExpectation()
        {
            const string theTouchpoint = "any old touchpoint";

            // arrange
            var request = MakeStrictMock<HttpRequest>();
            var trace = MakeStrictMock<ILogger>();
            var scope = MakeStrictMock<IScopeLoggingContext>();
            GetMock(scope)
                .Setup(x => x.Dispose());

            var factory = MakeStrictMock<ICreateLoggingContextScopes>();
            GetMock(factory)
                .Setup(x => x.BeginScopeFor(request, trace, "Run"))
                .Returns(Task.FromResult(scope));

            var adapter = MakeStrictMock<IManageAreaRoutingDetails>();
            GetMock(adapter)
                .Setup(x => x.DeleteAreaRoutingDetailUsing(theTouchpoint, scope))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            // act
            var result = await DeleteAreaRoutingDetailByTouchpointIDFunction.Run(request, trace, theTouchpoint, factory, adapter);

            // assert
            Assert.IsAssignableFrom<HttpResponseMessage>(result);
        }
    }
}
