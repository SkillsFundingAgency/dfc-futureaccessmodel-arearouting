﻿using System;
using System.Net;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using MarkEmbling.PostcodesIO.Exceptions;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Providers.Internal
{
    /// <summary>
    /// the fault response provider fixture
    /// </summary>
    public sealed class FaultResponseProviderFixture :
        MoqTestingFixture
    {
        const string fallbackContent = "{\"TouchpointID\":\"0000000999\",\"Area\":\"National Call Centre\",\"TelephoneNumber\":\"0800 123456\",\"SMSNumber\":\"\",\"EmailAddress\":\"nationalcareersservice@education.gov.uk\"}";

        /// <summary>
        /// the system under test supports it's service contract
        /// </summary>
        [Fact]
        public void TheSystemUnderTestSupportsItsServiceContract()
        {
            // arrange / act / assert
            Assert.IsAssignableFrom<IProvideFaultResponses>(MakeSUT());
        }

        /// <summary>
        /// get response for, using fallback, meets expectation
        /// </summary>
        /// <param name="testException">the test exception (type)</param>
        /// <param name="expectedMessage">the expected message recording</param>
        /// <returns>the current (test) task</returns>
        [Theory]
        [InlineData(typeof(MalformedRequestException), "")]
        [InlineData(typeof(NoContentException), "Resource does not exist")]
        [InlineData(typeof(InvalidPostcodeException), "Invalid postcode submitted")]        
        public async Task GetResponseForUsingFallbackMeetsExpectation(Type testException, string expectedMessage)
        {
            // arrange
            var sut = MakeSUT();
            var logger = MakeLoggingContext($"Exception of type '{testException.FullName}' was thrown.");
            GetMock(logger)
                .Setup(x => x.Information(expectedMessage))
                .Returns(Task.CompletedTask);

            var exception = (Exception)testException.Assembly.CreateInstance(testException.FullName);

            // act
            var result = await sut.GetResponseFor(exception, logger);

            // assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(fallbackContent, await result.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// get response for, with postcode exception, meets expectation
        /// </summary>
        /// <returns>the current (test) task</returns>
        [Fact]
        public async Task GetResponseForWithPostcodeExceptionMeetsExpectation()
        {
            // arrange
            var sut = MakeSUT();
            var logger = MakeLoggingContext($"Exception of type '{typeof(PostcodesIOApiException).FullName}' was thrown.");
            GetMock(logger)
                .Setup(x => x.Information($"Exception of type '{typeof(Exception).FullName}' was thrown."))
                .Returns(Task.CompletedTask);
            GetMock(logger)
                .Setup(x => x.Information("Error retrieving response. Please check inner exception for details."))
                .Returns(Task.CompletedTask);

            // act
            var result = await sut.GetResponseFor(new PostcodesIOApiException(new Exception()), logger);

            // assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(fallbackContent, await result.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// get response for, with postcode empty exception, meets expectation
        /// </summary>
        /// <returns>the current (test) task</returns>
        [Fact]
        public async Task GetResponseForWithPostcodeEmptyExceptionMeetsExpectation()
        {
            // arrange
            var sut = MakeSUT();
            var logger = MakeLoggingContext($"Exception of type '{typeof(PostcodesIOEmptyResponseException).FullName}' was thrown.");
            GetMock(logger)
                .Setup(x => x.Information("No response was provided; HTTP status: 501"))
                .Returns(Task.CompletedTask);

            // act
            var result = await sut.GetResponseFor(new PostcodesIOEmptyResponseException(HttpStatusCode.NotImplemented), logger);

            // assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(fallbackContent, await result.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// get response for the exception meets expectation
        /// </summary>
        /// <param name="testException">the test exception (type)</param>
        /// <param name="expectedState">the expected http response state</param>
        /// <param name="expectedMessage">the expected message recording</param>
        /// <returns>the current (test) task</returns>
        [Theory]
        [InlineData(typeof(UnauthorizedException), HttpStatusCode.Unauthorized, "")]
        [InlineData(typeof(AccessForbiddenException), HttpStatusCode.Forbidden, "Insufficient access to this resource")]
        [InlineData(typeof(UnprocessableEntityException), HttpStatusCode.UnprocessableEntity, "{ \"errors\": [{  }] }")]
        public async Task GetResponseForTheExceptionMeetsExpectation(Type testException, HttpStatusCode expectedState, string expectedMessage)
        {
            // arrange
            var sut = MakeSUT();
            var logger = MakeLoggingContext($"Exception of type '{testException.FullName}' was thrown.");
            GetMock(logger)
                .Setup(x => x.Information(expectedMessage))
                .Returns(Task.CompletedTask);

            var exception = (Exception)testException.Assembly.CreateInstance(testException.FullName);

            // act
            var result = await sut.GetResponseFor(exception, logger);

            // assert
            Assert.Equal(expectedState, result.StatusCode);
            Assert.Equal(expectedMessage, await result.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// get resonse for unknown exception meets expectation
        /// </summary>
        /// <param name="testException">the test exception (type)</param>
        /// <returns>the current (test) task</returns>
        [Theory]
        [InlineData(typeof(ArgumentNullException))]
        [InlineData(typeof(ArgumentException))]
        [InlineData(typeof(InvalidOperationException))]
        public async Task GetResponseForUnknownExceptionMeetsExpectation(Type testException)
        {
            // arrange
            var sut = MakeSUT();

            var exception = (Exception)testException.Assembly.CreateInstance(testException.FullName);

            var logger = MakeLoggingContext($"Exception of type '{testException.FullName}' was thrown.");
            GetMock(logger)
                .Setup(x => x.ExceptionDetail(exception))
                .Returns(Task.CompletedTask);

            // act
            var result = await sut.GetResponseFor(exception, logger);

            // assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(fallbackContent, await result.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// make scope logging context 
        /// </summary>
        /// <param name="itemBeingRecorded"></param>
        /// <returns>a logging context scope</returns>
        internal IScopeLoggingContext MakeLoggingContext(string itemBeingRecorded)
        {
            var logger = MakeStrictMock<IScopeLoggingContext>();
            GetMock(logger)
                .Setup(x => x.Information(itemBeingRecorded))
                .Returns(Task.CompletedTask);

            return logger;
        }

        /// <summary>
        /// make a 'system under test'
        /// </summary>
        /// <returns>the system under test</returns>
        internal FaultResponseProvider MakeSUT() =>
            new FaultResponseProvider();
    }
}
