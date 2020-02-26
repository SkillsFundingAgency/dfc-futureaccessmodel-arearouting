using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Models;
using Moq;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Validation.Internal
{
    /// <summary>
    /// routing detail validator fixture
    /// </summary>
    public sealed class RoutingDetailValidatorFixture :
        MoqTestingFixture
    {
        /// <summary>
        /// the system under test supports it's service contract
        /// </summary>
        [Fact]
        public void TheSystemUnderTestSupportsItsServiceContract()
        {
            // arrange / act / assert
            Assert.IsAssignableFrom<IValidateRoutingDetails>(MakeSUT());
        }

        /// <summary>
        /// build with null factory throws
        /// </summary>
        [Fact]
        public void BuildWithNullFactoryThrows()
        {
            // arrange / act / assert
            Assert.Throws<ArgumentNullException>(() => MakeSUT(null));
        }

        /// <summary>
        /// validate with null local authority throws
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task ValidateWithNullLocalAuthorityThrows()
        {
            // arrange
            var sut = MakeSUT();

            // act / assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Validate(null));
        }

        /// <summary>
        /// validate with invalid local authority throws
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task ValidateWithInvalidLocalAuthorityThrows()
        {
            // arrange
            var sut = MakeSUT();
            GetMock(sut.Message)
                .Setup(x => x.Create(It.IsAny<IReadOnlyCollection<ValidationResult>>()))
                .Returns("any old message content");

            // act / assert
            await Assert.ThrowsAsync<UnprocessableEntityException>(() => sut.Validate(new RoutingDetail()));
        }

        /// <summary>
        /// validate with valid local authority does not throw
        /// </summary>
        /// <returns>the currently running (test) task</returns>
        [Fact]
        public async Task ValidateWithValidLocalAuthorityDoesNotThrow()
        {
            // arrange
            var sut = MakeSUT();
            GetMock(sut.Message)
                .Setup(x => x.Create(It.IsAny<IReadOnlyCollection<ValidationResult>>()))
                .Returns("any old message content");

            // act
            await sut.Validate(new RoutingDetail { TouchpointID = "0000000100", Area = "any old area", EmailAddress = "blah@diblah.com", SMSNumber = "07123456789", TelephoneNumber = "01234567890" });

            // assert
            GetMock(sut.Message).VerifyAll();
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <returns>the system under test</returns>
        internal RoutingDetailValidator MakeSUT()
        {
            var factory = MakeStrictMock<ICreateValidationMessageContent>();

            return MakeSUT(factory);
        }

        /// <summary>
        /// make (a) 'system under test'
        /// </summary>
        /// <param name="paths">the storage paths provider</param>
        /// <param name="store">the document store</param>
        /// <returns>the system under test</returns>
        internal RoutingDetailValidator MakeSUT(
            ICreateValidationMessageContent factory) =>
                new RoutingDetailValidator(factory);
    }
}
