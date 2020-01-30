using System;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
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

        [Fact]
        public async Task CreateDocumentAsyncMeetsVerification()
        {
            // arrange
            const string touchPoint = "0000123";
            var sut = MakeSUT();
            var document = new RoutingDetail { TouchpointID = touchPoint };
            var collectionUri = new Uri("dbs/areas/colls/routing", UriKind.Relative);
            var documentUri = new Uri($"dbs/areas/colls/routing/docs/{touchPoint}", UriKind.Relative);

            GetMock(sut.Client)
                .Setup(x => x.CreateDocumentAsync(collectionUri, document, null, false, default))
                .Returns(Task.FromResult(new ResourceResponse<Document>(new Document())));
            GetMock(sut.Client)
                .Setup(x => x.ReadDocumentAsync<RoutingDetail>(documentUri, null, default))
                .Returns(Task.FromResult(new DocumentResponse<RoutingDetail>(document)));

            // act
            var result = await sut.CreateDocumentAsync(collectionUri, document);

            // assert
            Assert.IsAssignableFrom<IRoutingDetail>(result);
        }

        [Fact]
        public async Task DocumentExistsAsyncFalseWithNullResponseMeetsVerification()
        {
            // arrange
            var sut = MakeSUT();
            var documentUri = new Uri("dbs/areas/colls/routing/docs/0000123", UriKind.Relative);

            GetMock(sut.Client)
                .Setup(x => x.ReadDocumentAsync(documentUri, null, default))
                .Returns(Task.FromResult<ResourceResponse<Document>>(null));

            // act
            var result = await sut.DocumentExistsAsync(documentUri);

            // assert
            Assert.False(result);
        }

        [Fact]
        public async Task DocumentExistsAsyncTrueMeetsVerification()
        {
            // arrange
            var sut = MakeSUT();
            var documentUri = new Uri("dbs/areas/colls/routing/docs/0000123", UriKind.Relative);

            GetMock(sut.Client)
                .Setup(x => x.ReadDocumentAsync(documentUri, null, default))
                .Returns(Task.FromResult(new ResourceResponse<Document>(new Document())));

            // act
            var result = await sut.DocumentExistsAsync(documentUri);

            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task ReadDocumentAsyncWithValidURIMeetVerification()
        {
            // arrange
            var sut = MakeSUT();
            var documentUri = new Uri("dbs/areas/colls/routing/docs/0000123", UriKind.Relative);
            var document = new RoutingDetail();

            GetMock(sut.Client)
                .Setup(x => x.ReadDocumentAsync<RoutingDetail>(documentUri, null, default))
                .Returns(Task.FromResult(new DocumentResponse<RoutingDetail>(document)));

            // act
            var result = await sut.ReadDocumentAsync<RoutingDetail>(documentUri);

            // assert
            Assert.IsAssignableFrom<IRoutingDetail>(result);
        }

        [Theory]
        [InlineData("0000123", "dbs/areas/colls/routing")]
        [InlineData("E0600032", "dbs/regions/colls/authorities")]
        public void MakeDocumentPathForMeetsExpectation(string touchPoint, string collectionPath)
        {
            // arrange
            var document = new RoutingDetail { TouchpointID = touchPoint };
            var collectionUri = new Uri(collectionPath, UriKind.Relative);
            var documentPath = $"{collectionPath}/docs/{touchPoint}";

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
            new DocumentClientWrapper(MakeStrictMock<IDocumentClient>());
    }
}
