using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Storage;
using DFC.FutureAccessModel.AreaRouting.Wrappers;
using MarkEmbling.PostcodesIO.Results;
using System;
using System.Collections.Generic;
using Xunit;
using Task = System.Threading.Tasks.Task;

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
        /// <summary>
        /// build with factory and authorities meets verfication
        /// </summary>
        [Fact]
        public void BuildWithFactoryAndAuthoritiesMeetsVerification()
        {
            // arrange
            var client = MakeStrictMock<IWrapPostcodesClient>();
            var postcodes = MakeStrictMock<ICreatePostcodeClients>();
            GetMock(postcodes)
                .Setup(x => x.CreateClient())
                .Returns(client);

            var authorities = MakeStrictMock<IStoreLocalAuthorities>();

            // act
            var sut = new ExpressionActionProvider(postcodes, authorities);

            // assert
            Assert.Equal(client, sut.Postcode);
            Assert.Equal(authorities, sut.Authority);
        }

        /// <summary>
        /// action map contents meets expectation
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="expectation"></param>
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
        /// get action for expression type meets expectation
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="expectation"></param>
        [Theory]
        [InlineData(TypeOfExpression.Outward, "GetTouchpointIDFromOutwardCode")]
        [InlineData(TypeOfExpression.Postcode, "GetTouchpointIDFromPostcode")]
        [InlineData(TypeOfExpression.Town, "GetTouchpointIDFromTown")]
        [InlineData(TypeOfExpression.Unknown, "UnknownCandidateTypeAction")]
        public void GetActionForExpressionTypeMeetsExpectation(TypeOfExpression expression, string expectation)
        {
            // arrange
            var sut = MakeSUT();

            // act
            var result = sut.GetActionFor(expression);

            // assert
            Assert.Equal(expectation, result.Method.Name);
        }

        /// <summary>
        /// get touchpoint id from outward code with invalid candidate throws
        /// </summary>
        /// <param name="theCandidate"></param>
        /// <returns>the current (test) task</returns>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetTouchpointIDFromOutwardCodeWithEmptyCandidateThrows(string theCandidate)
        {
            // arrange
            var sut = MakeSUT();

            var theLoggingScope = MakeStrictMock<IScopeLoggingContext>();
            GetMock(theLoggingScope)
                .Setup(x => x.EnterMethod("GetTouchpointIDFromOutwardCode"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.EnterMethod("GetPostcodeUsing"))
                .Returns(Task.CompletedTask);

            // act / assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetTouchpointIDFromOutwardCode(theCandidate, theLoggingScope));
        }

        /// <summary>
        /// get outward code from (the psotcode) meets expectation
        /// </summary>
        /// <param name="theCandidate">the candidate (postcode)</param>
        /// <param name="theExpectation">the expectation</param>
        [Theory]
        [InlineData("SA38 9RD", "SA38")]
        [InlineData("SA389RD", "SA38")]
        [InlineData("NW1W 4ST", "NW1W")]
        [InlineData("NW1W4ST", "NW1W")]
        public void GetOutwardCodeFromMeetsExpectation(string theCandidate, string theExpectation)
        {
            // arrange
            var sut = MakeSUT();

            // act
            var result = sut.GetOutwardCodeFrom(theCandidate);

            // assert
            Assert.Equal(theExpectation, result);
        }

        /// <summary>
        /// get touchpoint id from outward code meets expectation
        /// </summary>
        /// <returns>the current (test) task</returns>
        [Fact]
        public async Task GetTouchpointIDFromOutwardCodeMeetsExpectation()
        {
            // arrange
            const string theOutwardCode = "SA38";
            const string thePostCode = "SA38 9RD";
            const string theAdminDistrict = "E06000060";
            const string theExpectation = "0000000456";

            var sut = MakeSUT();

            GetMock(sut.Postcode)
                .Setup(x => x.LookupOutwardCodeAsync(theOutwardCode, 10))
                .Returns(Task.FromResult<IReadOnlyCollection<string>>(new List<string> { thePostCode }));
            GetMock(sut.Postcode)
                .Setup(x => x.LookupAsync(thePostCode))
                .Returns(Task.FromResult(new PostcodeResult
                {
                    Postcode = thePostCode,
                    Codes = new Codes { AdminDistrict = theAdminDistrict }
                }));

            GetMock(sut.Authority)
                .Setup(x => x.Get(theAdminDistrict))
                .Returns(Task.FromResult<ILocalAuthority>(new LocalAuthority { LADCode = theAdminDistrict, TouchpointID = theExpectation }));

            var theLoggingScope = MakeStrictMock<IScopeLoggingContext>();
            GetMock(theLoggingScope)
                .Setup(x => x.EnterMethod("GetTouchpointIDFromOutwardCode"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.EnterMethod("GetPostcodeUsing"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.ExitMethod("GetPostcodeUsing"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.Information($"seeking postcode via outward code: '{theOutwardCode}'"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.ExitMethod("GetTouchpointIDFromOutwardCode"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.EnterMethod("GetTouchpointIDFromPostcode"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.Information($"seeking postcode '{thePostCode}'"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.Information($"found postcode for '{thePostCode}'"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.Information($"seeking local authority '{theAdminDistrict}'"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.Information($"found local authority '{theAdminDistrict}'"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.ExitMethod("GetTouchpointIDFromPostcode"))
                .Returns(Task.CompletedTask);

            // act
            var result = await sut.GetTouchpointIDFromOutwardCode(theOutwardCode, theLoggingScope);

            // assert
            Assert.Equal(theExpectation, result);
        }

        /// <summary>
        /// get touchpoint id from postcode with invalid postcode throws
        /// </summary>
        /// <returns>the current (test) task</returns>
        [Fact]
        public async Task GetTouchpointIDFromPostcodeWithInvalidPostcodeThrows()
        {
            // arrange
            const string thePostCode = "SA38 9RD";
            const string theOutwardCode = "SA38";

            var sut = MakeSUT();

            GetMock(sut.Postcode)
                .Setup(x => x.LookupAsync(thePostCode))
                .Returns(Task.FromResult((PostcodeResult)null));
            GetMock(sut.Postcode)
                .Setup(x => x.LookupOutwardCodeAsync(theOutwardCode, 10))
                .Returns(Task.FromResult<IReadOnlyCollection<string>>(new string[] { thePostCode }));

            var theLoggingScope = MakeStrictMock<IScopeLoggingContext>();
            GetMock(theLoggingScope)
                .Setup(x => x.EnterMethod("GetTouchpointIDFromPostcode"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.Information($"seeking postcode '{thePostCode}'"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.EnterMethod("GetPostcodeUsing"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.ExitMethod("GetPostcodeUsing"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.Information($"postcode search failed for: '{thePostCode}'"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.Information($"seeking postcode via outward code: '{theOutwardCode}'"))
                .Returns(Task.CompletedTask);

            // act / assert
            await Assert.ThrowsAsync<InvalidPostcodeException>(() => sut.GetTouchpointIDFromPostcode(thePostCode, theLoggingScope));
        }

        /// <summary>
        /// get postcode using invalid outward code throws no content exception
        /// </summary>
        /// <returns>the current (test) task</returns>
        [Fact]
        public async Task GetPostcodeUsingInvalidOutwardCodeThrowsNoContentException()
        {
            // arrange
            const string theOutwardCode = "SA38";

            var sut = MakeSUT();

            GetMock(sut.Postcode)
                .Setup(x => x.LookupOutwardCodeAsync(theOutwardCode, 10))
                .Returns(Task.FromResult<IReadOnlyCollection<string>>(new string[] { }));

            var theLoggingScope = MakeStrictMock<IScopeLoggingContext>();
            GetMock(theLoggingScope)
                .Setup(x => x.EnterMethod("GetPostcodeUsing"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.Information($"seeking postcode via outward code: '{theOutwardCode}'"))
                .Returns(Task.CompletedTask);

            // act / assert
            await Assert.ThrowsAsync<NoContentException>(() => sut.GetPostcodeUsing(theOutwardCode, theLoggingScope));
        }

        /// <summary>
        /// get touchpoint id from postcode with valid postcode meets expectation
        /// </summary>
        /// <returns>the current (test) task</returns>
        [Fact]
        public async Task GetTouchpointIDFromPostcodeWithValidPostcodeMeetsExpectation()
        {
            // arrange
            const string thePostCode = "SA38 9RD";
            const string theAdminDistrict = "E06000060";
            const string theExpectation = "0000000456";

            var sut = MakeSUT();

            GetMock(sut.Postcode)
                .Setup(x => x.LookupAsync(thePostCode))
                .Returns(Task.FromResult(new PostcodeResult
                {
                    Postcode = thePostCode,
                    Codes = new Codes { AdminDistrict = theAdminDistrict }
                }));

            GetMock(sut.Authority)
                .Setup(x => x.Get(theAdminDistrict))
                .Returns(Task.FromResult<ILocalAuthority>(new LocalAuthority { LADCode = theAdminDistrict, TouchpointID = theExpectation }));

            var theLoggingScope = MakeStrictMock<IScopeLoggingContext>();
            GetMock(theLoggingScope)
                .Setup(x => x.EnterMethod("GetTouchpointIDFromPostcode"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.Information($"seeking postcode '{thePostCode}'"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.Information($"found postcode for '{thePostCode}'"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.Information($"seeking local authority '{theAdminDistrict}'"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.Information($"found local authority '{theAdminDistrict}'"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.ExitMethod("GetTouchpointIDFromPostcode"))
                .Returns(Task.CompletedTask);

            // act
            var result = await sut.GetTouchpointIDFromPostcode(thePostCode, theLoggingScope);

            // assert
            Assert.Equal(theExpectation, result);
        }

        /// <summary>
        /// get touchpoint id from town currently throws not supported
        /// </summary>
        /// <returns>the current (test) task</returns>
        [Fact]
        public async Task GetTouchpointIDFromTownCurrentlyThrowsNotSupported()
        {
            // arrange
            const string theCandidate = "";

            var sut = MakeSUT();
            var theLoggingScope = MakeStrictMock<IScopeLoggingContext>();

            // act / assert
            await Assert.ThrowsAsync<NotSupportedException>(() => sut.GetTouchpointIDFromTown(theCandidate, theLoggingScope));
        }

        /// <summary>
        /// unknown candidate type action records and throws
        /// </summary>
        /// <returns>the current (test) task</returns>
        [Fact]
        public async Task UnknownCandidateTypeActionRecordsAndThrows()
        {
            // arrange
            const string thePostCode = "SA38 9RD";

            var sut = MakeSUT();

            GetMock(sut.Postcode)
                .Setup(x => x.LookupAsync(thePostCode))
                .Returns(Task.FromResult((PostcodeResult)null));

            var theLoggingScope = MakeStrictMock<IScopeLoggingContext>();
            GetMock(theLoggingScope)
                .Setup(x => x.EnterMethod("UnknownCandidateTypeAction"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.Information($"malformed request candidate: '{thePostCode}'"))
                .Returns(Task.CompletedTask);
            GetMock(theLoggingScope)
                .Setup(x => x.ExitMethod("UnknownCandidateTypeAction"))
                .Returns(Task.CompletedTask);

            // act / assert
            await Assert.ThrowsAsync<MalformedRequestException>(() => sut.UnknownCandidateTypeAction(thePostCode, theLoggingScope));
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
