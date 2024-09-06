using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Adapters;
using DFC.FutureAccessModel.AreaRouting.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Functions
{
    /// <summary>
    /// the get area routing detail by touchpoint ID function fixture
    /// </summary>
    public sealed class PostAreaRoutingDetailFunctionFixture :
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
            const string theContent = "{ \"TouchpointID\": \"00000000112\", \"SomeProperty\": \"Some Value...\" }";

            var request = MakeStrictMock<HttpRequest>();
            GetMock(request)
                .Setup(x => x.Body)
                .Returns(new MemoryStream(Encoding.UTF8.GetBytes(theContent)));

            var trace = MakeStrictMock<ILogger>();
            var scope = MakeStrictMock<IScopeLoggingContext>();
            GetMock(scope)
                .Setup(x => x.Dispose());

            var sut = MakeSUT();
            GetMock(sut.Factory)
                .Setup(x => x.BeginScopeFor(request, trace, "RunActionScope"))
                .Returns(Task.FromResult(scope));
            GetMock(sut.Adapter)
                .Setup(x => x.AddAreaRoutingDetailUsing(theContent, scope))
                .Returns(Task.FromResult<IActionResult>(new JsonResult(theContent, new JsonSerializerSettings())
                    { StatusCode = (int)HttpStatusCode.Created })
                );

            // act
            var result = await sut.Run(request, trace);
            var resultResponse = result as JsonResult;

            // assert
            Assert.Equal((int)HttpStatusCode.Created, resultResponse.StatusCode);
            Assert.IsAssignableFrom<IActionResult>(result);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <returns>the system under test</returns>
        internal PostAreaRoutingDetailFunction MakeSUT()
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
        internal PostAreaRoutingDetailFunction MakeSUT(
            ICreateLoggingContextScopes factory,
            IManageAreaRoutingDetails adapter) =>
                new PostAreaRoutingDetailFunction(factory, adapter);
    }
}
