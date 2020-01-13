using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using DFC.Swagger.Standard;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Functions
{
    /// <summary>
    /// the api (document generating) definition test fixture
    /// </summary>
    public sealed class ApiDefinitionFixture :
        MoqTestingFixture
    {
        /// <summary>
        /// the api definition title meets expectation
        /// </summary>
        [Fact]
        public void APIDefinitionTitleMeetsExpectation()
        {
            // arrange / act / assert
            Assert.Equal("areas", ApiDefinition.ApiTitle);
        }

        /// <summary>
        /// the api definition version meets expectation
        /// </summary>
        [Fact]
        public void APIDefinitionVersionMeetsExpectation()
        {
            // arrange / act / assert
            Assert.Equal("1.0.0", ApiDefinition.ApiVersion);
        }

        /// <summary>
        /// the api definition name meets expectation
        /// </summary>
        [Fact]
        public void APIDefinitionNameMeetsExpectation()
        {
            // arrange / act / assert
            Assert.Equal("api-definition", ApiDefinition.ApiDefinitionName);
        }

        /// <summary>
        /// the api definition description is not empty
        /// </summary>
        [Fact]
        public void APIDefinitionDescriptionIsNotEmpty()
        {
            // arrange / act / assert
            Assert.NotSame(string.Empty, ApiDefinition.ApiDescription);
        }

        /// <summary>
        /// the api definition run routine throws with a null http request
        /// </summary>
        [Fact]
        public void APIDefinitionRunWithNullRequestThrows()
        {
            // arrange
            var generator = MakeStrictMock<ISwaggerDocumentGenerator>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => ApiDefinition.Run(null, generator));
        }

        /// <summary>
        /// the api definition run routine throws with a null document generator
        /// </summary>
        [Fact]
        public void APIDefinitionRunWithNullDocumentGeneratorThrows()
        {
            // arrange
            var request = MakeStrictMock<HttpRequest>();

            // act / assert
            Assert.Throws<ArgumentNullException>(() => ApiDefinition.Run(request, null));
        }

        /// <summary>
        /// the api definition run routine throws with a null document generator
        /// </summary>
        [Fact]
        public async Task APIDefinitionRunMeetsExpectation()
        {
            const string documentContent = "document returned from generator";

            // arrange
            var request = MakeStrictMock<HttpRequest>();
            var generator = MakeStrictMock<ISwaggerDocumentGenerator>();

            // the mock expects the defaults to be sent in on optional values
            GetMock(generator)
                .Setup(x => x.GenerateSwaggerDocument(
                    request,
                    ApiDefinition.ApiTitle,
                    ApiDefinition.ApiDescription,
                    ApiDefinition.ApiDefinitionName,
                    ApiDefinition.ApiVersion,
                    Moq.It.IsAny<Assembly>(),
                    true, // include subcontractor id, optional parameter
                    true, // include touchpoint id, optional parameter
                    "/api/")) // api route prefix => /api, optional parameter
                .Returns(documentContent);

            // act
            var result = ApiDefinition.Run(request, generator);

            // assert
            Assert.IsAssignableFrom<HttpResponseMessage>(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(documentContent, await result.Content.ReadAsStringAsync());
        }
    }
}
