using DFC.FutureAccessModel.AreaRouting.Wrappers;
using DFC.FutureAccessModel.AreaRouting.Wrappers.Internal;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Factories.Internal
{
    /// <summary>
    ///  the postcodes client factory fixture
    /// </summary>
    public sealed class PostcodeClientFactoryFixture :
        MoqTestingFixture
    {
        /// <summary>
        /// the system under test supports it's service contract
        /// </summary>
        [Fact]
        public void TheSystemUnderTestSupportsItsServiceContract()
        {
            // arrange / act / assert
            Assert.IsAssignableFrom<ICreatePostcodeClients>(MakeSUT());
        }

        /// <summary>
        /// create client returns something
        /// </summary>
        [Fact]
        public void CreatClientReturnsSomething()
        {
            // arrange
            var sut = MakeSUT();

            // act
            var client = sut.CreateClient();

            // assert
            Assert.NotNull(client);
        }

        /// <summary>
        /// create client returns a postcodes client
        /// </summary>
        [Fact]
        public void CreatClientReturnsADocumentClient()
        {
            // arrange
            var sut = MakeSUT();

            // act
            var client = sut.CreateClient();

            // assert
            Assert.IsType<PostcodesClientWrapper>(client);
        }

        /// <summary>
        /// create client returns an instance using the 'i wrap postcodes client' contract
        /// </summary>
        [Fact]
        public void CreatClientReturnValueIsAssignableContract()
        {
            // arrange
            var sut = MakeSUT();

            // act
            var client = sut.CreateClient();

            // assert
            Assert.IsAssignableFrom<IWrapPostcodesClient>(client);
        }

        /// <summary>
        /// make a 'system under test'
        /// </summary>
        /// <returns>a document client factory</returns>
        internal PostcodeClientFactory MakeSUT() =>
            new PostcodeClientFactory();
    }
}
