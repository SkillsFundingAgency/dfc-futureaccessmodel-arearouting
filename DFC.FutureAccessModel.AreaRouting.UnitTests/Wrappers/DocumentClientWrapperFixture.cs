using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Moq;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Wrappers.Internal
{
    public class DocumentClientWrapperFixture :
        MoqTestingFixture
    {
        /// <summary>
        /// the system under test supports it's service contract
        /// </summary>
        [Fact]
        public void TheSystemUnderTestSupportsItsServiceContract()
        {
            // arrange / act / assert
            Assert.IsAssignableFrom<IWrapDocumentClient>(MakeSUT());
        }

        /// <summary>
        /// build with null client throws
        /// </summary>
        [Fact]
        public void BuildWithNullClientThrows()
        {
            // arrange / act / assert
            Assert.Throws<ArgumentNullException>(() =>  MakeSUT(null));
        }

        /// <summary>
        /// build with client meets expectation
        /// </summary>
        [Fact]
        public void BuildWithClientMeetsExpectation()
        {
            // arrange
            var client = MakeStrictMock<IDocumentClient>();

            // act
            var sut = MakeSUT(client);

            // assert
            Assert.Equal(client, sut.Client);
        }

        /// <summary>
        /// create document (async) meets expectation
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task CreateDocumentAsyncMeetsExpectation()
        {
            // arrange
            const string keyValue = "0000123";
            var sut = MakeSUT();
            var document = new IncomingRoutingDetail { TouchpointID = keyValue };
            var collectionUri = new Uri("dbs/areas/colls/routing", UriKind.Relative);
            var documentUri = new Uri($"dbs/areas/colls/routing/docs/{keyValue}", UriKind.Relative);

            GetMock(sut.Client)
                .Setup(x => x.CreateDocumentAsync(collectionUri, document, null, false, default))
                .Returns(Task.FromResult(new ResourceResponse<Document>(new Document())));
            GetMock(sut.Client)
                .Setup(x => x.ReadDocumentAsync<IncomingRoutingDetail>(documentUri, It.IsAny<RequestOptions>(), default))
                .Returns(Task.FromResult(new DocumentResponse<IncomingRoutingDetail>(document)));

            // act
            var result = await sut.CreateDocumentAsync(collectionUri, document);

            // assert
            Assert.IsAssignableFrom<IRoutingDetail>(result);
        }

        /// <summary>
        /// document exists (async) false with null response meets expectation
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task DocumentExistsAsyncFalseWithNullResponseMeetsExpectation()
        {
            // arrange
            var sut = MakeSUT();
            var documentUri = new Uri("dbs/areas/colls/routing/docs/0000123", UriKind.Relative);

            GetMock(sut.Client)
                .Setup(x => x.ReadDocumentAsync<RoutingDetail>(documentUri, It.IsAny<RequestOptions>(), default))
                .Returns(Task.FromResult<DocumentResponse<RoutingDetail>>(null));

            // act
            var result = await sut.DocumentExistsAsync<RoutingDetail>(documentUri, string.Empty);

            // assert
            Assert.False(result);
        }

        /// <summary>
        /// document exists (async) true meets expectation
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task DocumentExistsAsyncTrueMeetsExpectation()
        {
            // arrange
            var sut = MakeSUT();
            var documentUri = new Uri("dbs/areas/colls/routing/docs/0000123", UriKind.Relative);

            GetMock(sut.Client)
                .Setup(x => x.ReadDocumentAsync<RoutingDetail>(documentUri, It.IsAny<RequestOptions>(), default))
                .Returns(Task.FromResult(new DocumentResponse<RoutingDetail>(new RoutingDetail())));

            // act
            var result = await sut.DocumentExistsAsync<RoutingDetail>(documentUri, string.Empty);

            // assert
            Assert.True(result);
        }

        /// <summary>
        /// read document (async) with valid URI meets expectation
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task ReadDocumentAsyncWithValidURIMeetsExpectation()
        {
            // arrange
            var sut = MakeSUT();
            var documentUri = new Uri("dbs/areas/colls/routing/docs/0000123", UriKind.Relative);
            var document = new LocalAuthority();

            GetMock(sut.Client)
                .Setup(x => x.ReadDocumentAsync<LocalAuthority>(documentUri, It.IsAny<RequestOptions>(), default))
                .Returns(Task.FromResult(new DocumentResponse<LocalAuthority>(document)));

            // act
            var result = await sut.ReadDocumentAsync<LocalAuthority>(documentUri, string.Empty);

            // assert
            Assert.IsAssignableFrom<ILocalAuthority>(result);
        }

        /// <summary>
        /// delete document with valid uri meets verfication
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task DeleteDocumentAsyncWithValidURIMeetsVerification()
        {
            // arrange
            var sut = MakeSUT();
            var documentUri = new Uri("dbs/areas/colls/routing/docs/0000123", UriKind.Relative);
            var document = new LocalAuthority();

            GetMock(sut.Client)
                .Setup(x => x.DeleteDocumentAsync(documentUri, It.IsAny<RequestOptions>(), default))
                .Returns(Task.FromResult(new ResourceResponse<Document>()));

            // act
            await sut.DeleteDocumentAsync(documentUri, string.Empty);

            // assert
            GetMock(sut.Client).VerifyAll();
        }

        /// <summary>
        /// create document query meet verification
        /// </summary>
        [Fact(Skip = "under consideration: can't get this work work due to casting issues inside 'as document query'")]
        public void CreateDocumentQueryMeetsVerification()
        {
            // arrange
            const string sqlCommand = "any old SQL command";
            var collectionPath = new Uri("dbs/areas/colls/routing", UriKind.Relative);

            var sut = MakeSUT();
            GetMock(sut.Client)
                .Setup(x => x.CreateDocumentQuery<RoutingDetail>(collectionPath, sqlCommand, It.IsAny<FeedOptions>()))
                .Returns(new List<RoutingDetail>().AsQueryable());

            // act
            var result = sut.CreateDocumentQuery<RoutingDetail>(collectionPath, sqlCommand);

            // assert
            GetMock(sut.Client).VerifyAll();
            Assert.IsAssignableFrom<IDocumentQuery<RoutingDetail>>(result);
        }

        /// <summary>
        /// make document path for key value and collection meets expectation
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="collectionPath"></param>
        [Theory]
        [InlineData("0000123", "dbs/areas/colls/routing")]
        [InlineData("E0600032", "dbs/regions/colls/authorities")]
        public void MakeDocumentPathForKeyValueAndCollectionMeetsExpectation(string keyValue, string collectionPath)
        {
            // arrange
            var document = new LocalAuthority { LADCode = keyValue };
            var collectionUri = new Uri(collectionPath, UriKind.Relative);
            var documentPath = $"{collectionPath}/docs/{keyValue}";

            var sut = MakeSUT();

            // act
            var result = sut.MakeDocumentPathFor(document, collectionUri);

            // assert
            Assert.Equal(documentPath, result.OriginalString);
        }

        /// <summary>
        /// make a 'system under test'
        /// </summary>
        /// <returns>a system under test</returns>
        internal DocumentClientWrapper MakeSUT() =>
            MakeSUT(MakeStrictMock<IDocumentClient>());

        /// <summary>
        /// make a 'system under test'
        /// </summary>
        /// <returns>a system under test</returns>
        internal DocumentClientWrapper MakeSUT(IDocumentClient client) =>
            new DocumentClientWrapper(client);
    }
}
