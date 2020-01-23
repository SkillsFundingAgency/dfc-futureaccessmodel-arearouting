using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Models;
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
        /// build with null response helper throws
        /// </summary>
        [Fact]
        public void BuildWithNullResponseHelperThrows()
        {
            // arrange
            var store = MakeStrictMock<IStoreAreaRoutingDetails>();
            var faults = MakeStrictMock<IProvideFaultResponses>();
            var safe = MakeStrictMock<IProvideSafeOperations>();
            var analyser = MakeStrictMock<IAnalyseExpresssions>();
            var actions = MakeStrictMock<IProvideExpressionActions>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => new AreaRoutingDetailManagementFunctionAdapter(null, faults, safe, store, analyser, actions));
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
            var analyser = MakeStrictMock<IAnalyseExpresssions>();
            var actions = MakeStrictMock<IProvideExpressionActions>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => new AreaRoutingDetailManagementFunctionAdapter(helper, null, safe, store, analyser, actions));
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
            var analyser = MakeStrictMock<IAnalyseExpresssions>();
            var actions = MakeStrictMock<IProvideExpressionActions>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => new AreaRoutingDetailManagementFunctionAdapter(helper, faults, null, store, analyser, actions));
        }

        /// <summary>
        /// build with null (document) storage (provider) throws
        /// </summary>
        [Fact]
        public void BuildWithNullStorageProviderThrows()
        {
            // arrange
            var helper = MakeStrictMock<IHttpResponseMessageHelper>();
            var faults = MakeStrictMock<IProvideFaultResponses>();
            var safe = MakeStrictMock<IProvideSafeOperations>();
            var analyser = MakeStrictMock<IAnalyseExpresssions>();
            var actions = MakeStrictMock<IProvideExpressionActions>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => new AreaRoutingDetailManagementFunctionAdapter(helper, faults, safe, null, analyser, actions));
        }

        [Fact]
        public void BuildWithNullExpressionAnalyserThrows()
        {
            // arrange
            var store = MakeStrictMock<IStoreAreaRoutingDetails>();
            var helper = MakeStrictMock<IHttpResponseMessageHelper>();
            var faults = MakeStrictMock<IProvideFaultResponses>();
            var safe = MakeStrictMock<IProvideSafeOperations>();
            var actions = MakeStrictMock<IProvideExpressionActions>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => new AreaRoutingDetailManagementFunctionAdapter(helper, faults, safe, store, null, actions));
        }

        [Fact]
        public void BuildWithNullActionProviderThrows()
        {
            // arrange
            var store = MakeStrictMock<IStoreAreaRoutingDetails>();
            var helper = MakeStrictMock<IHttpResponseMessageHelper>();
            var faults = MakeStrictMock<IProvideFaultResponses>();
            var safe = MakeStrictMock<IProvideSafeOperations>();
            var analyser = MakeStrictMock<IAnalyseExpresssions>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => new AreaRoutingDetailManagementFunctionAdapter(helper, faults, safe, store, analyser, null));
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
            GetMock(sut.RoutingDetails).VerifyAll();
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
            await Assert.ThrowsAsync<MalformedRequestException>(() => sut.ProcessGetAreaRoutingDetailFor(touchpointID, scope));
        }

        [Fact]
        public async Task GetAreaRoutingDetailForValidTouchpointMeetsVerification()
        {
            // arrange
            const string touchpoint = "any old touchpoint";
            const string resultText = "{\"TouchpointID\":null,\"Area\":null,\"TelephoneNumber\":null,\"SMSNumber\":null,\"EmailAddress\":null}";

            var sut = MakeSUT();
            GetMock(sut.RoutingDetails)
                .Setup(x => x.Get(touchpoint))
                .Returns(Task.FromResult<IRoutingDetail>(new RoutingDetail()));
            GetMock(sut.Respond)
                .Setup(x => x.Ok(resultText))
                .Returns(new HttpResponseMessage());

            var scope = MakeStrictMock<IScopeLoggingContext>();
            GetMock(scope)
                .Setup(x => x.EnterMethod("ProcessGetAreaRoutingDetailFor"))
                .Returns(Task.CompletedTask);
            GetMock(scope)
                .Setup(x => x.ExitMethod("ProcessGetAreaRoutingDetailFor"))
                .Returns(Task.CompletedTask);

            // act
            var result = await sut.ProcessGetAreaRoutingDetailFor(touchpoint, scope);

            // assert
            Assert.IsAssignableFrom<HttpResponseMessage>(result);
            GetMock(sut.RoutingDetails).VerifyAll();
            GetMock(sut.Respond).VerifyAll();
            GetMock(sut.Faults).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();
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
            GetMock(sut.RoutingDetails).VerifyAll();
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

        [Fact]
        public async Task GetAreaRoutingDetailByValidLocationMeetsVerification()
        {
            // arrange
            const string locationIn = "any old location";
            const string touchpointOut = "any old touchpoint";
            const string resultText = "{\"TouchpointID\":null,\"Area\":null,\"TelephoneNumber\":null,\"SMSNumber\":null,\"EmailAddress\":null}";

            var sut = MakeSUT();
            GetMock(sut.Analyser)
                .Setup(x => x.GetTypeOfExpressionFor(locationIn))
                .Returns(TypeOfExpression.Postcode);
            GetMock(sut.Actions)
                .Setup(x => x.GetActionFor(It.IsAny<TypeOfExpression>()))
                .Returns((x, y) => Task.FromResult(touchpointOut));
            GetMock(sut.RoutingDetails)
                .Setup(x => x.Get(touchpointOut))
                .Returns(Task.FromResult<IRoutingDetail>(new RoutingDetail()));
            GetMock(sut.Respond)
                .Setup(x => x.Ok(resultText))
                .Returns(new HttpResponseMessage());

            var scope = MakeStrictMock<IScopeLoggingContext>();
            GetMock(scope)
                .Setup(x => x.EnterMethod("ProcessGetAreaRoutingDetailBy"))
                .Returns(Task.CompletedTask);
            GetMock(scope)
                .Setup(x => x.ExitMethod("ProcessGetAreaRoutingDetailBy"))
                .Returns(Task.CompletedTask);
            GetMock(scope)
                .Setup(x => x.EnterMethod("ProcessGetAreaRoutingDetailFor"))
                .Returns(Task.CompletedTask);
            GetMock(scope)
                .Setup(x => x.ExitMethod("ProcessGetAreaRoutingDetailFor"))
                .Returns(Task.CompletedTask);

            // act
            var result = await sut.ProcessGetAreaRoutingDetailBy(locationIn, scope);

            // assert
            Assert.IsAssignableFrom<HttpResponseMessage>(result);
            GetMock(sut.RoutingDetails).VerifyAll();
            GetMock(sut.Respond).VerifyAll();
            GetMock(sut.Faults).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <returns>the system under test</returns>
        internal AreaRoutingDetailManagementFunctionAdapter MakeSUT()
        {
            var store = MakeStrictMock<IStoreAreaRoutingDetails>();
            var helper = MakeStrictMock<IHttpResponseMessageHelper>();
            var faults = MakeStrictMock<IProvideFaultResponses>();
            var safe = MakeStrictMock<IProvideSafeOperations>();
            var analyser = MakeStrictMock<IAnalyseExpresssions>();
            var actions = MakeStrictMock<IProvideExpressionActions>();

            return MakeSUT(store, helper, faults, safe, analyser, actions);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <param name="store">the store</param>
        /// <param name="helper">the (response) helper</param>
        /// <param name="faults">the fault (response provider)</param>
        /// <param name="safe">the safe (operations provider)</param>
        /// <returns>the system under test</returns>
        internal AreaRoutingDetailManagementFunctionAdapter MakeSUT(
            IStoreAreaRoutingDetails store,
            IHttpResponseMessageHelper helper,
            IProvideFaultResponses faults,
            IProvideSafeOperations safe,
            IAnalyseExpresssions analyser,
            IProvideExpressionActions actions) =>
                new AreaRoutingDetailManagementFunctionAdapter(helper, faults, safe, store, analyser, actions);
    }
}
