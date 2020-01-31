using System;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Storage;
using DFC.FutureAccessModel.AreaRouting.Wrappers;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Providers.Internal
{
    /// <summary>
    /// the expression action provider fixture
    /// </summary>
    public sealed class ExpressionActionProviderFixture :
        MoqTestingFixture
    {
        /// <summary>
        /// the system under test supports it's service contract
        /// </summary>
        [Fact]
        public void TheSystemUnderTestSupportsItsServiceContract()
        {
            // arrange / act / assert
            Assert.IsAssignableFrom<IProvideExpressionActions>(MakeSUT());
        }

        /// <summary>
        /// build with null postcodes client factory throws
        /// </summary>
        [Fact]
        public void BuildWithNullPostcodesClientFactoryThrows()
        {
            // arrange
            var authorities = MakeStrictMock<IStoreLocalAuthorities>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => new ExpressionActionProvider(null, authorities));
        }

        /// <summary>
        /// build with null authority provider throws
        /// </summary>
        [Fact]
        public void BuildWithNullAuthorityProviderThrows()
        {
            // arrange
            var postcodes = MakeStrictMock<ICreatePostcodeClients>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => new ExpressionActionProvider(postcodes, null));
        }

        [Theory]
        [InlineData(TypeOfExpression.Outward, true)]
        [InlineData(TypeOfExpression.Postcode, true)]
        [InlineData(TypeOfExpression.Town, true)]
        [InlineData(TypeOfExpression.Unknown, false)]
        public void ActionMapContentsMeetsExpectation(TypeOfExpression expression, bool expectation)
        {
            // arrange
            var sut = MakeSUT();

            // act
            var result = sut.ActionMap.ContainsKey(expression);

            // assert
            Assert.Equal(expectation, result);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <returns>the system under test</returns>
        internal ExpressionActionProvider MakeSUT()
        {
            var postcodes = MakeStrictMock<ICreatePostcodeClients>();
            GetMock(postcodes)
                .Setup(x => x.CreateClient())
                .Returns(MakeStrictMock<IWrapPostcodesClient>());

            var authorities = MakeStrictMock<IStoreLocalAuthorities>();

            return MakeSUT(postcodes, authorities);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <param name="factory">the postcodes client factory</param>
        /// <param name="authorities">the authorities store</param>
        /// <returns>the system under test</returns>
        internal ExpressionActionProvider MakeSUT(
            ICreatePostcodeClients factory,
            IStoreLocalAuthorities authorities) =>
            new ExpressionActionProvider(factory, authorities);
    }
}
