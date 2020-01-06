using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Storage.Internal
{
    public class DocumentDBFixture :
        MoqTestingFixture
    {
        /// <summary>
        /// the document db connection detail meets expectation
        /// </summary>
        [Fact]
        public void DocumentDBConnectionDetailMeetsExpectation()
        {
            // arrange / act / assert
            Assert.Equal("AreaRoutingStorageConnectionDetail", DocumentDB.DocumentStoreConnectionDetailKey);
        }

        [Fact]
        public void DocumentDBValueSeparatorKeyMeetsExpectation()
        {
            // arrange / act / assert
            Assert.Equal("=", DocumentDB.ValueSeparatorKey);
        }

        [Fact]
        public void DocumentDBValueTerminatorKeyMeetsExpectation()
        {
            // arrange / act / assert
            Assert.Equal(";", DocumentDB.ValueTerminatorKey);
        }

        [Fact]
        public void DocumentDBEndpointAddressMeetsExpectation()
        {
            // arrange / act / assert
            Assert.Equal(";", DocumentDB.EndpointAddress);
        }
    }
}
