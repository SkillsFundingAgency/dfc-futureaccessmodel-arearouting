using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Providers;
using DFC.FutureAccessModel.AreaRouting.Storage;
using DFC.HTTP.Standard;
using Moq;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Adapters.Internal
{
    public sealed class GetAreaRoutingDetailFunctionAdapterFixture :
        MoqTestingFixture

    {
        /// <summary>
        /// build with null (document) storage (provider) throws
        /// </summary>
        [Fact]
        public void BuildWithNullTraceThrows()
        {
            // arrange
            var helper = MakeStrictMock<IHttpResponseMessageHelper>();
            var faults = MakeStrictMock<IProvideFaultResponses>();
            var safe = MakeStrictMock<IProvideSafeOperations>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => new GetAreaRoutingDetailFunctionAdapter(null, helper, faults, safe));
        }

        /// <summary>
        /// build with null response helper throws
        /// </summary>
        [Fact]
        public void BuildWithNullResponseHelperThrows()
        {
            // arrange
            var store = MakeStrictMock<IStoreAreaRoutingDetails>();
            var faults = MakeStrictMock<IProvideFaultResponses>();
            var safe = MakeStrictMock<IProvideSafeOperations>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => new GetAreaRoutingDetailFunctionAdapter(store, null, faults, safe));
        }

        /// <summary>
        /// build with null fault response provider throws
        /// </summary>
        [Fact]
        public void BuildWithNullFaultsResponseProviderThrows()
        {
            // arrange
            var store = MakeStrictMock<IStoreAreaRoutingDetails>();
            var helper = MakeStrictMock<IHttpResponseMessageHelper>();
            var safe = MakeStrictMock<IProvideSafeOperations>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => new GetAreaRoutingDetailFunctionAdapter(store, helper, null, safe));
        }

        /// <summary>
        /// build with null safe opeations throws
        /// </summary>
        [Fact]
        public void BuildWithNullSafeOperationsThrows()
        {
            // arrange
            var store = MakeStrictMock<IStoreAreaRoutingDetails>();
            var helper = MakeStrictMock<IHttpResponseMessageHelper>();
            var faults = MakeStrictMock<IProvideFaultResponses>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => new GetAreaRoutingDetailFunctionAdapter(store, helper, faults, null));
        }

        /// <summary>
        /// get area routing detail for, meets verification
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAreaRoutingDetailForMeetsVerification()
        {
            // arrange
            var sut = MakeSUT();
            var scope = MakeStrictMock<IScopeLoggingContext>();

            GetMock(sut.SafeOperations)
                .Setup(x => x.Try(It.IsAny<Func<Task<HttpResponseMessage>>>(), It.IsAny<Func<Exception, Task<HttpResponseMessage>>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            // act
            var result = await sut.GetAreaRoutingDetailFor(string.Empty, scope);

            // assert
            Assert.IsAssignableFrom<HttpResponseMessage>(result);
            GetMock(sut.StorageProvider).VerifyAll();
            GetMock(sut.Respond).VerifyAll();
            GetMock(sut.Faults).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();
        }

        /// <summary>
        /// process get area routing detail for invalid touchpoint id meets expectation
        /// </summary>
        /// <param name="touchpointID"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ProcessGetAreaRoutingDetailForInvalidTouchpointIDMeetsExpectation(string touchpointID)
        {
            // arrange
            var sut = MakeSUT();

            var scope = MakeStrictMock<IScopeLoggingContext>();
            GetMock(scope)
                .Setup(x => x.EnterMethod("ProcessGetAreaRoutingDetailFor"))
                .Returns(Task.CompletedTask);

            // act / assert
            await Assert.ThrowsAsync<MalformedRequestException>(()=> sut.ProcessGetAreaRoutingDetailFor(touchpointID, scope));
        }

        /// <summary>
        /// get area routing detail by, meets verification
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAreaRoutingDetailByMeetsVerification()
        {
            // arrange
            var sut = MakeSUT();
            var scope = MakeStrictMock<IScopeLoggingContext>();

            GetMock(sut.SafeOperations)
                .Setup(x => x.Try(It.IsAny<Func<Task<HttpResponseMessage>>>(), It.IsAny<Func<Exception, Task<HttpResponseMessage>>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            // act
            var result = await sut.GetAreaRoutingDetailBy(string.Empty, scope);

            // assert
            Assert.IsAssignableFrom<HttpResponseMessage>(result);
            GetMock(sut.StorageProvider).VerifyAll();
            GetMock(sut.Respond).VerifyAll();
            GetMock(sut.Faults).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();
        }

        /// <summary>
        /// process get area routing detail by invalid location meets expectation
        /// </summary>
        /// <param name="location">the location</param>
        /// <returns></returns>
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ProcessGetAreaRoutingDetailByInvalidLocationMeetsExpectation(string location)
        {
            // arrange
            var sut = MakeSUT();

            var scope = MakeStrictMock<IScopeLoggingContext>();
            GetMock(scope)
                .Setup(x => x.EnterMethod("ProcessGetAreaRoutingDetailBy"))
                .Returns(Task.CompletedTask);

            // act / assert
            await Assert.ThrowsAsync<MalformedRequestException>(() => sut.ProcessGetAreaRoutingDetailBy(location, scope));
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <returns>the system under test</returns>
        internal GetAreaRoutingDetailFunctionAdapter MakeSUT()
        {
            var store = MakeStrictMock<IStoreAreaRoutingDetails>();
            var helper = MakeStrictMock<IHttpResponseMessageHelper>();
            var faults = MakeStrictMock<IProvideFaultResponses>();
            var safe = MakeStrictMock<IProvideSafeOperations>();

            return MakeSUT(store, helper, faults, safe);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <param name="store">the store</param>
        /// <param name="helper">the (response) helper</param>
        /// <param name="faults">the fault (response provider)</param>
        /// <param name="safe">the safe (operations provider)</param>
        /// <returns>the system under test</returns>
        internal GetAreaRoutingDetailFunctionAdapter MakeSUT(
            IStoreAreaRoutingDetails store,
            IHttpResponseMessageHelper helper,
            IProvideFaultResponses faults,
            IProvideSafeOperations safe) =>
                new GetAreaRoutingDetailFunctionAdapter(store, helper, faults, safe);
    }
}
