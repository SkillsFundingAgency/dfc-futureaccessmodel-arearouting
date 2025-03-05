using MarkEmbling.PostcodesIO;
using MarkEmbling.PostcodesIO.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Wrappers.Internal
{
    /// <summary>
    /// the postcodes client wrapper fixture
    /// </summary>
    public class PostcodesClientWrapperFixture :
        MoqTestingFixture
    {
        /// <summary>
        /// the system under test supports it's service contract
        /// </summary>
        [Fact]
        public void TheSystemUnderTestSupportsItsServiceContract()
        {
            // arrange / act / assert
            Assert.IsAssignableFrom<IWrapPostcodesClient>(MakeSUT());
        }

        /// <summary>
        /// build with null client throws
        /// </summary>
        [Fact]
        public void BuildWithNullClientThrows()
        {
            // arrange / act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(null));
        }

        /// <summary>
        /// build with client meets expectation
        /// </summary>
        [Fact]
        public void BuildWithClientMeetsExpectation()
        {
            // arrange
            var client = MakeStrictMock<IPostcodesIOClient>();

            // act
            var sut = MakeSUT(client);

            // assert
            Assert.Equal(client, sut.Client);
        }

        /// <summary>
        /// lookup async meets expectation
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task LookupAsyncMeetsExpectation()
        {
            // arrange
            const string thePostcode = "any old postcode";
            var sut = MakeSUT();

            GetMock(sut.Client)
                .Setup(x => x.LookupAsync(thePostcode))
                .Returns(Task.FromResult(new PostcodeResult()));

            // act
            var result = await sut.LookupAsync(thePostcode);

            // assert
            Assert.IsType<PostcodeResult>(result);
        }

        /// <summary>
        /// lookup outward code async meets expectation
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task LookupOutwardCodeAsyncMeetsExpectation()
        {
            // arrange
            const string theOutcode = "any old outcode";
            const int theResultsSize = 15;

            var sut = MakeSUT();

            GetMock(sut.Client)
                .Setup(x => x.AutocompleteAsync(theOutcode, theResultsSize))
                .Returns(Task.FromResult(Enumerable.Empty<string>()));

            // act
            var result = await sut.LookupOutwardCodeAsync(theOutcode, theResultsSize);

            // assert
            Assert.Empty(result);
            Assert.IsAssignableFrom<IReadOnlyCollection<string>>(result);
        }

        /// <summary>
        /// validate async meets expectation
        /// </summary>
        /// <param name="expectation"></param>
        /// <returns>the currently running (test) task</returns>
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task ValidateAsyncMeetsExpectation(bool expectation)
        {
            // arrange
            const string thePostcode = "any old postcode";
            var sut = MakeSUT();

            GetMock(sut.Client)
                .Setup(x => x.ValidateAsync(thePostcode))
                .Returns(Task.FromResult(expectation));

            // act
            var result = await sut.ValidateAsync(thePostcode);

            // assert
            Assert.Equal(expectation, result);
        }

        /// <summary>
        /// make a 'system under test'
        /// </summary>
        /// <returns>a system under test</returns>
        internal PostcodesClientWrapper MakeSUT() =>
            MakeSUT(MakeStrictMock<IPostcodesIOClient>());

        /// <summary>
        /// make a 'system under test'
        /// </summary>
        /// <returns>a system under test</returns>
        internal PostcodesClientWrapper MakeSUT(IPostcodesIOClient client) =>
            new PostcodesClientWrapper(client);
    }
}
