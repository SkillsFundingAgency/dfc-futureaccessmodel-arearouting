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
using Moq;
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
            var logger = MakeStrictMock<ILogger<PostAreaRoutingDetailFunction>>();

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
            var logger = MakeStrictMock<ILogger<PostAreaRoutingDetailFunction>>();

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
            const string theContent = "{ \"TouchpointID\": \"00000000112\", \"SomeProperty\": \"Some Value...\" }";
            
            var factory = new Mock<ICreateLoggingContextScopes>();
            var adapter = new Mock<IManageAreaRoutingDetails>();
            var logger = new Mock<ILogger<PostAreaRoutingDetailFunction>>();
            var scope = new Mock<IScopeLoggingContext>();
            var request = new Mock<HttpRequest>();
            
            var sut = new PostAreaRoutingDetailFunction(factory.Object, adapter.Object, logger.Object);
            
            GetMock(request.Object)
                .Setup(x => x.Body)
                .Returns(new MemoryStream(Encoding.UTF8.GetBytes(theContent)));
            GetMock(scope.Object)
                .Setup(x => x.Dispose());
            GetMock(sut.Factory)
                .Setup(x => x.BeginScopeFor(request.Object, logger.Object, "RunActionScope"))
                .Returns(Task.FromResult(scope.Object));
            GetMock(sut.Adapter)
                .Setup(x => x.AddAreaRoutingDetailUsing(theContent, scope.Object))
                .Returns(Task.FromResult<IActionResult>(new JsonResult(theContent, new JsonSerializerSettings())
                { StatusCode = (int)HttpStatusCode.Created })
                );

            // act
            var result = await sut.Run(request.Object);
            var resultResponse = result as JsonResult;

            // assert
            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.Equal((int)HttpStatusCode.Created, resultResponse.StatusCode);
            Assert.NotNull(resultResponse.Value);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <returns>the system under test</returns>
        internal PostAreaRoutingDetailFunction MakeSUT()
        {
            var factory = MakeStrictMock<ICreateLoggingContextScopes>();
            var adapter = MakeStrictMock<IManageAreaRoutingDetails>();
            var logger = MakeStrictMock<ILogger<PostAreaRoutingDetailFunction>>();

            return MakeSUT(factory, adapter, logger);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <param name="factory">the logging scope factory</param>
        /// <param name="adapter">the area routing management function adapter</param>
        /// <returns>the system under test</returns>
        internal PostAreaRoutingDetailFunction MakeSUT(
            ICreateLoggingContextScopes factory,
            IManageAreaRoutingDetails adapter,
            ILogger<PostAreaRoutingDetailFunction> logger) =>
                new PostAreaRoutingDetailFunction(factory, adapter, logger);
    }
}
