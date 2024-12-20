﻿using DFC.FutureAccessModel.AreaRouting.Factories.Internal;
using DFC.FutureAccessModel.AreaRouting.Wrappers;
using DFC.FutureAccessModel.AreaRouting.Wrappers.Internal;
using System;
using Xunit;

namespace DFC.FutureAccessModel.AreaRouting.Factories
{
    public sealed class DocumentClientFactoryFixture :
        MoqTestingFixture
    {
        /// <summary>
        /// test account key has to be base 64
        /// </summary>
        const string _testAccountKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        /// <summary>
        /// test document storage path
        /// </summary>
        Uri _testPath = new Uri("http://testDomain/testStore");

        /// <summary>
        /// the system under test supports it's service contract
        /// </summary>
        [Fact]
        public void TheSystemUnderTestSupportsItsServiceContract()
        {
            // arrange / act / assert
            Assert.IsAssignableFrom<ICreateDocumentClients>(MakeSUT());
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
            var client = sut.CreateClient(_testPath, _testAccountKey);

            // assert
            Assert.NotNull(client);
        }

        /// <summary>
        /// create client returns a document client wrapper
        /// </summary>
        [Fact]
        public void CreatClientReturnsADocumentClient()
        {
            // arrange
            var sut = MakeSUT();

            // act
            var client = sut.CreateClient(_testPath, _testAccountKey);

            // assert
            Assert.IsType<DocumentClientWrapper>(client);
        }

        /// <summary>
        /// create client returns an instance using the 'i wrap document client' contract
        /// </summary>
        [Fact]
        public void CreatClientReturnValueIsAssignableContract()
        {
            // arrange
            var sut = MakeSUT();

            // act
            var client = sut.CreateClient(_testPath, _testAccountKey);

            // assert
            Assert.IsAssignableFrom<IWrapDocumentClient>(client);
        }

        /// <summary>
        /// make a 'system under test'
        /// </summary>
        /// <returns>a system under test</returns>
        internal DocumentClientFactory MakeSUT() =>
            new DocumentClientFactory();
    }
}
