﻿using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Providers;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Moq;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Storage.Internal
{
    public sealed class DocumentStoreFixture :
        MoqTestingFixture
    {
        const string storeEndpointAddressKey = "DocumentStoreEndpointAddress";
        const string storeAccountKey = "DocumentStoreAccountKey";

        /// <summary>
        /// the system under test supports it's service contract
        /// </summary>
        [Fact]
        public void TheSystemUnderTestSupportsItsServiceContract()
        {
            // arrange / act / assert
            Assert.IsAssignableFrom<IStoreDocuments>(MakeSUT());
        }

        /// <summary>
        /// the document store endpoint address key meets expectation
        /// </summary>
        [Fact]
        public void DocumentStoreEndpointAddressKeyMeetsExpectation()
        {
            // arrange / act / assert
            Assert.Equal(storeEndpointAddressKey, DocumentStore.DocumentStoreEndpointAddressKey);
        }

        [Fact]
        public void DocumentStoreAccountKeyMeetsExpectation()
        {
            // arrange / act / assert
            Assert.Equal(storeAccountKey, DocumentStore.DocumentStoreAccountKey);
        }

        /// <summary>
        /// new document store build fails with null settings
        /// </summary>
        [Fact]
        public void NewDocumentStoreBuildFailsWithNullSettings()
        {
            // arrange
            var factory = MakeStrictMock<ICreateDocumentClients>();
            var safeOps = MakeStrictMock<IProvideSafeOperations>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(null, factory, safeOps));
        }

        /// <summary>
        /// new document store build fails with null settings
        /// </summary>
        [Fact]
        public void NewDocumentStoreBuildFailsWithNullFactory()
        {
            // arrange
            var settings = MakeStrictMock<IProvideApplicationSettings>();
            var safeOps = MakeStrictMock<IProvideSafeOperations>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(settings, null, safeOps));
        }

        /// <summary>
        /// new document store build fails with null safe operations
        /// </summary>
        [Fact]
        public void NewDocumentStoreBuildFailsWithNullSafeOperations()
        {
            // arrange
            var settings = MakeStrictMock<IProvideApplicationSettings>();
            var factory = MakeStrictMock<ICreateDocumentClients>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(settings, factory, null));
        }

        /// <summary>
        /// new document store build failure meets expectation
        /// </summary>
        /// <param name="endpointAddress">the test endpoint address</param>
        /// <param name="accountKey">the test account key</param>
        /// <param name="expectedException">the expected exception</param>
        [Theory]
        [InlineData(null, null, typeof(ArgumentNullException))]
        [InlineData(null, "anyOldAccountKey", typeof(ArgumentNullException))]
        [InlineData("anyOldEndpoint", null, typeof(ArgumentNullException))]
        [InlineData("anyOldEndpoint", "anyOldAccountKey", typeof(UriFormatException))]
        public void NewDocumentStoreBuildFailureMeetsExpectation(string endpointAddress, string accountKey, Type expectedException)
        {
            // arrange
            var settings = MakeStrictMock<IProvideApplicationSettings>();
            GetMock(settings)
                .Setup(x => x.GetVariable(storeEndpointAddressKey))
                .Returns(endpointAddress);
            GetMock(settings)
                .Setup(x => x.GetVariable(storeAccountKey))
                .Returns(accountKey);

            var factory = MakeStrictMock<ICreateDocumentClients>();
            var safeOps = MakeStrictMock<IProvideSafeOperations>();

            // act / assert
            Assert.Throws(expectedException, () => MakeSUT(settings, factory, safeOps));
        }

        /// <summary>
        /// new document store meets verification
        /// </summary>
        [Fact]
        public void NewDocumentStoreBuildMeetsVerification()
        {
            // arrange
            const string accountKeyValue = "anyOldAccountKey";

            var settings = MakeStrictMock<IProvideApplicationSettings>();
            GetMock(settings)
                .Setup(x => x.GetVariable(storeEndpointAddressKey))
                .Returns("http://localhost:123/");
            GetMock(settings)
                .Setup(x => x.GetVariable(storeAccountKey))
                .Returns(accountKeyValue);

            var factory = MakeStrictMock<ICreateDocumentClients>();
            GetMock(factory)
                .Setup(x => x.CreateClient(It.IsAny<Uri>(), accountKeyValue))
                .Returns<IDocumentClient>(null);

            var safeOps = MakeStrictMock<IProvideSafeOperations>();

            // act
            var sut = MakeSUT(settings, factory, safeOps);

            // assert
            GetMock(settings).VerifyAll();
            GetMock(factory).VerifyAll();
            GetMock(safeOps).VerifyAll();
        }

        /// <summary>
        /// add document meets verification
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task AddDocumentMeetsVerification()
        {
            // arrange
            var sut = MakeSUT();
            var modelItem = new RoutingDetail();
            var testPath = new Uri("http://blahStore/blahCollection/blahID");

            GetMock(sut.Client)
                .Setup(x => x.CreateDocumentAsync(testPath, modelItem, null, false, default))
                .Returns(Task.FromResult(new ResourceResponse<Document>()));

            // act
            await sut.AddDocument(modelItem, testPath);

            // assert
            GetMock(sut.Client).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();
        }

        /// <summary>
        /// get document meets verification
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task GetDocumentMeetsVerification()
        {
            // arrange
            var sut = MakeSUT();
            var testPath = new Uri("http://blahStore/blahCollection/blahID");

            GetMock(sut.SafeOperations)
                .Setup(x => x.Try(It.IsAny<Func<Task<RoutingDetail>>>(), It.IsAny<Func<Exception, Task<RoutingDetail>>>()))
                .Returns(Task.FromResult(new RoutingDetail()));

            // act
            var result = await sut.GetDocument<RoutingDetail>(testPath);

            // assert
            GetMock(sut.Client).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();
            Assert.IsType<RoutingDetail>(result);
        }

        /// <summary>
        /// process get docuement meets expectation
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task ProcessGetDocumentMeetsVerification()
        {
            // arrange
            var sut = MakeSUT();
            var testPath = new Uri("http://blahStore/blahCollection/blahID");

            GetMock(sut.Client)
                .Setup(x => x.ReadDocumentAsync(testPath, null, default))
                .Returns(Task.FromResult(new ResourceResponse<Document>(new Document())));

            // act
            var result = await sut.ProcessGetDocument<RoutingDetail>(testPath);

            // assert
            GetMock(sut.Client).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();
            Assert.IsType<RoutingDetail>(result);
        }

        /// <summary>
        /// process get document error handler throws malformed request exception for null incoming exception
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task ProcessGetDocumentErrorHandlerThrowsMalformedRequestForIncomingNull()
        {
            // arrange
            var sut = MakeSUT();

            // act / assert
            await Assert.ThrowsAsync<MalformedRequestException>(() => sut.ProcessGetDocumentErrorHandler<RoutingDetail>(null));
        }

        /// <summary>
        /// process get document error handler meets expectation
        /// </summary>
        /// <param name="httpCode">the http code</param>
        /// <param name="expectedException">the expected exception</param>
        /// <returns>the currently running (test) task</returns>
        [Theory]
        [InlineData((int)HttpStatusCode.NotFound, typeof(NoContentException))]
        [InlineData((int)LocalHttpStatusCode.TooManyRequests, typeof(MalformedRequestException))]
        public async Task ProcessGetDocumentErrorHandlerMeetsExpectation(int httpCode, Type expectedException)
        {
            // arrange
            var sut = MakeSUT();
            var exception = MakeDocumentClientException((HttpStatusCode)httpCode);

            // act / assert
            await Assert.ThrowsAsync(expectedException, () => sut.ProcessGetDocumentErrorHandler<RoutingDetail>(exception));
        }

        /// <summary>
        /// make (a) document client exception
        /// all constructors have been internalised for some reason so we can't mock or 'new' up
        /// </summary>
        /// <param name="httpStatusCode"></param>
        /// <returns>a document client exception</returns>
        internal DocumentClientException MakeDocumentClientException(HttpStatusCode httpStatusCode)
        {
            var type = typeof(DocumentClientException);
            var name = type.FullName;
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var parms = new object[] { new Error(), null, httpStatusCode };

            var instance = type.Assembly.CreateInstance(name, false, flags, null, parms, null, null);

            return (DocumentClientException)instance;
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <returns>the system under test</returns>
        internal DocumentStore MakeSUT()
        {
            const string accountKeyValue = "anyOldAccountKey";

            var settings = MakeStrictMock<IProvideApplicationSettings>();
            GetMock(settings)
                .Setup(x => x.GetVariable(storeEndpointAddressKey))
                .Returns("http://localhost:123/");
            GetMock(settings)
                .Setup(x => x.GetVariable(storeAccountKey))
                .Returns(accountKeyValue);

            var factory = MakeStrictMock<ICreateDocumentClients>();
            GetMock(factory)
                .Setup(x => x.CreateClient(It.IsAny<Uri>(), accountKeyValue))
                .Returns(MakeStrictMock<IDocumentClient>());

            var safeOperator = MakeStrictMock<IProvideSafeOperations>();

            return MakeSUT(settings, factory, safeOperator);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <param name="settings">the application settings provider</param>
        /// <param name="factory">the document client factory</param>
        /// <param name="safeOperator">the safe operations provider</param>
        /// <returns>the system under test</returns>
        internal DocumentStore MakeSUT(
            IProvideApplicationSettings settings,
            ICreateDocumentClients factory,
            IProvideSafeOperations safeOperator)
        {
            return new DocumentStore(settings, factory, safeOperator);
        }
    }
}
