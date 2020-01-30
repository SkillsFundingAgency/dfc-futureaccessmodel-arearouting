using MarkEmbling.PostcodesIO;
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
        /// make a 'system under test'
        /// </summary>
        /// <returns>a system under test</returns>
        internal PostcodesClientWrapper MakeSUT() =>
            new PostcodesClientWrapper(MakeStrictMock<IPostcodesIOClient>());
    }
}
