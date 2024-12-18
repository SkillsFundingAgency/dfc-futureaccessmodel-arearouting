using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Providers;
using DFC.FutureAccessModel.AreaRouting.Wrappers;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Storage.Internal
{
    /// <summary>
    /// the document store fixture
    /// </summary>
    public sealed class DocumentStoreFixture :
        MoqTestingFixture
    {
        /// <summary>
        ///  the endpoint address key
        /// </summary>
        const string storeEndpointAddressKey = "DocumentStoreEndpointAddress";

        /// <summary>
        /// the document store key
        /// </summary>
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
        public void EndpointAddressKeyMeetsExpectation()
        {
            // arrange / act / assert
            Assert.Equal(storeEndpointAddressKey, DocumentStore.DocumentStoreEndpointAddressKey);
        }

        /// <summary>
        /// the document store account key meets expectation
        /// </summary>
        [Fact]
        public void AccountKeyMeetsExpectation()
        {
            // arrange / act / assert
            Assert.Equal(storeAccountKey, DocumentStore.DocumentStoreAccountKey);
        }

        /// <summary>
        /// build with null settings throws
        /// </summary>
        [Fact]
        public void BuildWithNullSettingsThrows()
        {
            // arrange
            var factory = MakeStrictMock<ICreateDocumentClients>();
            var safeOps = MakeStrictMock<IProvideSafeOperations>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(null, factory, safeOps));
        }

        /// <summary>
        /// build with null factory throws
        /// </summary>
        [Fact]
        public void BuildWithNullFactoryThrows()
        {
            // arrange
            var settings = MakeStrictMock<IProvideApplicationSettings>();
            var safeOps = MakeStrictMock<IProvideSafeOperations>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(settings, null, safeOps));
        }

        /// <summary>
        /// build with null safe operations throws
        /// </summary>
        [Fact]
        public void BuildWithNullSafeOperationsThrows()
        {
            // arrange
            var settings = MakeStrictMock<IProvideApplicationSettings>();
            var factory = MakeStrictMock<ICreateDocumentClients>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(settings, factory, null));
        }

        /// <summary>
        /// build failure meets expectation
        /// </summary>
        /// <param name="endpointAddress">the test endpoint address</param>
        /// <param name="accountKey">the test account key</param>
        /// <param name="expectedException">the expected exception</param>
        [Theory]
        [InlineData(null, null, typeof(ArgumentNullException))]
        [InlineData(null, "anyOldAccountKey", typeof(ArgumentNullException))]
        [InlineData("anyOldEndpoint", null, typeof(ArgumentNullException))]
        [InlineData("anyOldEndpoint", "anyOldAccountKey", typeof(UriFormatException))]
        public void BuildFailureMeetsExpectation(string endpointAddress, string accountKey, Type expectedException)
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
        /// build meets verification
        /// </summary>
        [Fact]
        public void BuildMeetsVerification()
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
        /// document exists meets expectation
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DocumentExistsMeetsExpectation(bool expectation)
        {
            // arrange
            var testPath = new Uri("http://blahStore/blahCollection/blahID");

            var sut = MakeSUT();

            GetMock(sut.SafeOperations)
                .Setup(x => x.Try(It.IsAny<Func<Task<bool>>>(), It.IsAny<Func<Exception, Task<bool>>>()))
                .Returns(Task.FromResult(expectation));

            // act
            var result = await sut.DocumentExists<RoutingDetail>(testPath, string.Empty);

            // assert
            GetMock(sut.Client).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();

            Assert.Equal(expectation, result);
        }

        /// <summary>
        /// process document exists meets verification
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ProcessDocumentExistsMeetsVerification(bool expectation)
        {
            // arrange
            const string pKey = "any old partition key";
            var testPath = new Uri("http://blahStore/blahCollection/blahID");

            var sut = MakeSUT();

            GetMock(sut.Client)
                .Setup(x => x.DocumentExistsAsync<RoutingDetail>(testPath, pKey))
                .Returns(Task.FromResult(expectation));

            // act
            var result = await sut.ProcessDocumentExists<RoutingDetail>(testPath, pKey);

            // assert
            GetMock(sut.Client).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();

            Assert.Equal(expectation, result);
        }

        /// <summary>
        /// add document meets verification
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task AddDocumentMeetsVerification()
        {
            // arrange
            var modelItem = new RoutingDetail();
            var testPath = new Uri("http://blahStore/blahCollection/blahID");

            var sut = MakeSUT();

            GetMock(sut.SafeOperations)
                .Setup(x => x.Try(It.IsAny<Func<Task<RoutingDetail>>>(), It.IsAny<Func<Exception, Task<RoutingDetail>>>()))
                .Returns(Task.FromResult(modelItem));

            // act
            await sut.AddDocument(modelItem, testPath);

            // assert
            GetMock(sut.Client).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();
        }

        /// <summary>
        /// process add document meets verification
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task ProcessAddDocumentMeetsVerification()
        {
            // arrange
            var modelItem = new RoutingDetail();
            var testPath = new Uri("http://blahStore/blahCollection/blahID");

            var sut = MakeSUT();

            GetMock(sut.Client)
                .Setup(x => x.CreateDocumentAsync(testPath, modelItem))
                .Returns(Task.FromResult(modelItem));

            // act
            await sut.ProcessAddDocument(testPath, modelItem);

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
            const string pKey = "any old partition key";
            var testPath = new Uri("http://blahStore/blahCollection/blahID");

            var sut = MakeSUT();

            GetMock(sut.SafeOperations)
                .Setup(x => x.Try(It.IsAny<Func<Task<RoutingDetail>>>(), It.IsAny<Func<Exception, Task<RoutingDetail>>>()))
                .Returns(Task.FromResult(new RoutingDetail()));

            // act
            var result = await sut.GetDocument<RoutingDetail>(testPath, pKey);

            // assert
            GetMock(sut.Client).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();
            Assert.IsType<RoutingDetail>(result);
        }

        /// <summary>
        /// process get document meets expectation
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task ProcessGetDocumentMeetsVerification()
        {
            // arrange
            const string pKey = "any old partition key";
            var testPath = new Uri("http://blahStore/blahCollection/blahID");

            var sut = MakeSUT();

            GetMock(sut.Client)
                .Setup(x => x.ReadDocumentAsync<RoutingDetail>(testPath, pKey))
                .Returns(Task.FromResult(new RoutingDetail()));

            // act
            var result = await sut.ProcessGetDocument<RoutingDetail>(testPath, pKey);

            // assert
            GetMock(sut.Client).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();
            Assert.IsType<RoutingDetail>(result);
        }

        [Fact]
        public async Task CreateDocumentQueryMeetsVerification()
        {
            // arrange
            var testPath = new Uri("http://blahStore/blahCollection");

            var sut = MakeSUT();

            GetMock(sut.SafeOperations)
                .Setup(x => x.Try(
                    It.IsAny<Func<Task<IReadOnlyCollection<RoutingDetail>>>>(),
                    It.IsAny<Func<Exception, Task<IReadOnlyCollection<RoutingDetail>>>>()))
                .Returns(Task.FromResult<IReadOnlyCollection<RoutingDetail>>(new List<RoutingDetail>()));

            // act
            var result = await sut.CreateDocumentQuery<RoutingDetail>(testPath);

            // assert
            GetMock(sut.Client).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();
        }

        [Fact]
        public async Task ProcessCreateDocumentQueryMeetsVerification()
        {
            // arrange
            const string sqlCommand = "any old SQL command";
            var testPath = new Uri("http://blahStore/blahCollection");

            var documentQuery = MakeStrictMock<IDocumentQuery<RoutingDetail>>();
            GetMock(documentQuery)
                .Setup(x => x.Dispose());
            GetMock(documentQuery)
                .Setup(x => x.HasMoreResults)
                .ReturnsInOrder(true, false);
            GetMock(documentQuery)
                .Setup(x => x.ExecuteNextAsync<RoutingDetail>(CancellationToken.None))
                .Returns(Task.FromResult(new FeedResponse<RoutingDetail>(new RoutingDetail[] { new RoutingDetail { }, new RoutingDetail { } })));

            var sut = MakeSUT();
            GetMock(sut.Client)
                .Setup(x => x.CreateDocumentQuery<RoutingDetail>(testPath, sqlCommand))
                .Returns(documentQuery);

            // act
            var result = await sut.ProcessCreateDocumentQuery<RoutingDetail>(testPath, sqlCommand);

            // assert
            Assert.Equal(2, result.Count);
            GetMock(documentQuery)
                .Verify(x => x.ExecuteNextAsync<RoutingDetail>(CancellationToken.None), Times.Exactly(1));
            GetMock(sut.Client).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();
        }

        /// <summary>
        /// delete document meets verification
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task DeleteDocumentMeetsVerification()
        {
            // arrange
            const string pKey = "any old partition key";
            var testPath = new Uri("http://blahStore/blahCollection/blahID");

            var sut = MakeSUT();

            GetMock(sut.SafeOperations)
                .Setup(x => x.Try(It.IsAny<Func<Task>>(), It.IsAny<Func<Exception, Task>>()))
                .Returns(Task.CompletedTask);

            // act
            await sut.DeleteDocument(testPath, pKey);

            // assert
            GetMock(sut.Client).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();
        }

        /// <summary>
        /// process delete document meets verification
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task ProcessDeleteDocumentMeetsVerification()
        {
            // arrange
            const string pKey = "any old partition key";
            var testPath = new Uri("http://blahStore/blahCollection/blahID");

            var sut = MakeSUT();

            GetMock(sut.Client)
                .Setup(x => x.DeleteDocumentAsync(testPath, pKey))
                .Returns(Task.CompletedTask);

            // act
            await sut.ProcessDeleteDocument(testPath, pKey);

            // assert
            GetMock(sut.Client).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();
        }

        /// <summary>
        /// process document error handler throws malformed request exception for null incoming exception
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task ProcessDocumentErrorHandlerThrowsMalformedRequestForIncomingNull()
        {
            // arrange
            var sut = MakeSUT();

            // act / assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ProcessDocumentErrorHandler<RoutingDetail>(null));
        }

        /// <summary>
        /// process document error handler meets expectation
        /// </summary>
        /// <param name="httpCode">the http code</param>
        /// <param name="expectedException">the expected exception</param>
        /// <returns>the currently running (test) task</returns>
        [Theory]
        [InlineData(HttpStatusCode.NotFound, typeof(NoContentException))]
        [InlineData(HttpStatusCode.Conflict, typeof(ConflictingResourceException))]
        [InlineData(HttpStatusCode.TooManyRequests, typeof(DocumentClientException))]
        [InlineData(HttpStatusCode.RequestTimeout, typeof(DocumentClientException))]
        public async Task ProcessDocumentErrorHandlerMeetsExpectation(HttpStatusCode httpCode, Type expectedException)
        {
            // arrange
            var sut = MakeSUT();
            var exception = MakeDocumentClientException(httpCode);

            // act / assert
            await Assert.ThrowsAsync(expectedException, () => sut.ProcessDocumentErrorHandler<RoutingDetail>(exception));
        }

        /// <summary>
        /// process document error handler with null http status code
        /// this shouldn't happen and is the test in place only for code coverage
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task ProcessDocumentErrorHandlerWithNullHttpStatusCode()
        {
            // arrange
            var sut = MakeSUT();
            var exception = MakeDocumentClientException(null);

            // act / assert
            await Assert.ThrowsAsync<DocumentClientException>(() => sut.ProcessDocumentErrorHandler<RoutingDetail>(exception));
        }

        /// <summary>
        /// process document client error with null passes witout event
        /// this shouldn't happen and is the test in place only for code coverage
        /// </summary>
        [Fact]
        public void ProcessDocumentClientErrorWithNullPassesWithoutEvent()
        {
            // arrange
            var sut = MakeSUT();

            // act
            sut.ProcessDocumentClientError(null);

            // assert
            GetMock(sut.Client).VerifyAll();
            GetMock(sut.SafeOperations).VerifyAll();
        }

        /// <summary>
        /// make (a) document client exception
        /// all constructors have been internalised for some reason so we can't mock or 'new' up
        /// </summary>
        /// <param name="httpStatusCode"></param>
        /// <returns>a document client exception</returns>
        internal DocumentClientException MakeDocumentClientException(HttpStatusCode? httpStatusCode)
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
                .Returns(MakeStrictMock<IWrapDocumentClient>());

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
            IProvideSafeOperations safeOperator) =>
                new DocumentStore(settings, factory, safeOperator);
    }
}
