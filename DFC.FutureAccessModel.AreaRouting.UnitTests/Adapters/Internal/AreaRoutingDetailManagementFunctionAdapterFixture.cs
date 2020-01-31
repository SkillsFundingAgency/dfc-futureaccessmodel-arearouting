﻿using System;
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
    public sealed class AreaRoutingDetailManagementFunctionAdapterFixture :
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

        /// <summary>
        /// build with null expression analyser throws
        /// </summary>
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

        /// <summary>
        /// build with null action provider throws
        /// </summary>
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
        /// <returns>the currently running (test) task</returns>
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
        /// <returns>the currently running (test) task</returns>
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

        /// <summary>
        /// process get area routing details for valid touchpoint meets verification
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task ProcessGetAreaRoutingDetailForValidTouchpointMeetsVerification()
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
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task GetAreaRoutingDetailByMeetsVerification()
        {
            // arrange
            var sut = MakeSUT();
            var scope = MakeStrictMock<IScopeLoggingContext>();

            GetMock(sut.SafeOperations)
                .Setup(x => x.Try(
                    It.IsAny<Func<Task<HttpResponseMessage>>>(),
                    It.IsAny<Func<Exception, Task<HttpResponseMessage>>>()))
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
        /// <returns>the currently running (test) task</returns>
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
        /// process get area routing detail by valid location meets verification
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task ProcessGetAreaRoutingDetailByValidLocationMeetsVerification()
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
        /// add area routing detail using, meets verification 
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task AddAreaRoutingDetailUsingMeetsVerification()
        {
            // arrange
            var sut = MakeSUT();
            var scope = MakeStrictMock<IScopeLoggingContext>();

            GetMock(sut.SafeOperations)
                .Setup(x => x.Try(
                    It.IsAny<Func<Task<HttpResponseMessage>>>(),
                    It.IsAny<Func<Exception, Task<HttpResponseMessage>>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            // act
            var result = await sut.AddAreaRoutingDetailUsing(string.Empty, scope);

            // assert
            Assert.IsAssignableFrom<HttpResponseMessage>(result);
            GetMock(sut.RoutingDetails).VerifyAll();
            GetMock(sut.Respond).VerifyAll();
            GetMock(sut.Faults).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();
        }

        /// <summary>
        /// process add routing detail using missing content throws
        /// </summary>
        /// <param name="theContent">the content</param>
        /// <returns>the currently running (test) task</returns>
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ProcessAddAreaRoutingDetailUsingMissingContentThrows(string theContent)
        {
            // arrange
            var sut = MakeSUT();

            var scope = MakeStrictMock<IScopeLoggingContext>();
            GetMock(scope)
                .Setup(x => x.EnterMethod("ProcessAddAreaRoutingDetailUsing"))
                .Returns(Task.CompletedTask);

            // act / assert
            await Assert.ThrowsAsync<MalformedRequestException>(() => sut.ProcessAddAreaRoutingDetailUsing(theContent, scope));
        }

        /// <summary>
        /// process add routing detail using invalid content throws
        /// </summary>
        /// <param name="theContent">the content</param>
        /// <returns>the currently running (test) task</returns>
        [Theory]
        [InlineData("{ }")]
        [InlineData("{ \"TouchpointID\": null}")]
        public async Task ProcessAddAreaRoutingDetailUsingInvalidContentThrows(string theContent)
        {
            // arrange
            var sut = MakeSUT();

            var scope = MakeStrictMock<IScopeLoggingContext>();
            GetMock(scope)
                .Setup(x => x.EnterMethod("ProcessAddAreaRoutingDetailUsing"))
                .Returns(Task.CompletedTask);
            GetMock(scope)
                .Setup(x => x.Information($"deserialising the submitted content: {theContent}"))
                .Returns(Task.CompletedTask);
            GetMock(scope)
                .Setup(x => x.Information("deserialisation complete..."))
                .Returns(Task.CompletedTask);

            // act / assert
            await Assert.ThrowsAsync<MalformedRequestException>(() => sut.ProcessAddAreaRoutingDetailUsing(theContent, scope));
        }

        /// <summary>
        /// process add area routing detail using valid content meets verification
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task ProcessAddAreaRoutingDetailUsingValidContentMeetsVerification()
        {
            // arrange
            const string theTouchpoint = "0000000123";
            var theContent = $"{{\"TouchpointID\":\"{theTouchpoint}\", \"Area\": null, \"TelephoneNumber\": null, \"SMSNumber\": null, \"EmailAddress\": null }}";

            var sut = MakeSUT();
            GetMock(sut.RoutingDetails)
                .Setup(x => x.Add(It.IsAny<RoutingDetail>()))
                .Returns(Task.FromResult<IRoutingDetail>(new RoutingDetail()));
            GetMock(sut.Respond)
                .Setup(x => x.Created())
                .Returns(new HttpResponseMessage());

            var scope = MakeStrictMock<IScopeLoggingContext>();
            GetMock(scope)
                .Setup(x => x.EnterMethod("ProcessAddAreaRoutingDetailUsing"))
                .Returns(Task.CompletedTask);
            GetMock(scope)
                .Setup(x => x.Information($"deserialising the submitted content: {theContent}"))
                .Returns(Task.CompletedTask);
            GetMock(scope)
                .Setup(x => x.Information("deserialisation complete..."))
                .Returns(Task.CompletedTask);
            GetMock(scope)
                .Setup(x => x.Information($"adding the area routing candidate: {theTouchpoint}"))
                .Returns(Task.CompletedTask);
            GetMock(scope)
                .Setup(x => x.Information($"candidate addition complete..."))
                .Returns(Task.CompletedTask);
            GetMock(scope)
                .Setup(x => x.ExitMethod("ProcessAddAreaRoutingDetailUsing"))
                .Returns(Task.CompletedTask);

            // act
            var result = await sut.ProcessAddAreaRoutingDetailUsing(theContent, scope);

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