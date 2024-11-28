using DFC.FutureAccessModel.AreaRouting.Models;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Providers.Internal
{
    /// <summary>
    /// the expression analyser fixture
    /// </summary>
    public sealed class ExpressionAnalyserFixture
    {
        /// <summary>
        /// the system under test supports it's service contract
        /// </summary>
        [Fact]
        public void TheSystemUnderTestSupportsItsServiceContract()
        {
            // arrange / act / assert
            Assert.IsAssignableFrom<IAnalyseExpresssions>(MakeSUT());
        }

        [Theory]
        [InlineData("CV1", TypeOfExpression.Outward)]
        [InlineData("CV1 4JP", TypeOfExpression.Postcode)]
        [InlineData("Leicester", TypeOfExpression.Town)]
        [InlineData("Leicester13", TypeOfExpression.Unknown)]
        [InlineData("CV1 ", TypeOfExpression.Unknown)]
        [InlineData("CV1 4JPP", TypeOfExpression.Unknown)]
        [InlineData("CV13 4QP", TypeOfExpression.Postcode)]
        [InlineData("CV13 44P", TypeOfExpression.Unknown)]
        public void MeetsExpectation(string theCandidate, TypeOfExpression expectation)
        {
            // arrange
            var sut = MakeSUT();

            // act
            var result = sut.GetTypeOfExpressionFor(theCandidate);

            // assert
            Assert.Equal(expectation, result);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <returns>the system under test</returns>
        internal ExpressionAnalyser MakeSUT() =>
            new ExpressionAnalyser();
    }
}
