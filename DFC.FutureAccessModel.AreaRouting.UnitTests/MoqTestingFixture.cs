using System.IO;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DFC.FutureAccessModel.AreaRouting
{
    public abstract class MoqTestingFixture
    {
        //protected IConfiguration Config { get; }

        //protected MoqTestingFixture()
        //{
        //    Config = LoadConfiguration();
        //}

        //public static IConfiguration LoadConfiguration() =>
        //    new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("test.settings.json", false, true)
        //        .AddEnvironmentVariables()
        //        .Build();

        /// <summary>
        /// make strict mock
        /// </summary>
        /// <typeparam name="TEntity">for this type</typeparam>
        /// <returns>a strict behaviour mock</returns>
        public TEntity MakeStrictMock<TEntity>()
            where TEntity : class =>
            new Mock<TEntity>(MockBehavior.Strict).Object;

        /// <summary>
        /// get mock
        /// </summary>
        /// <typeparam name="TEntity">the type</typeparam>
        /// <param name="forItem">for this instance of <typeparamref name="TEntity"/>the type</param>
        /// <returns>the mock</returns>
        public Mock<TEntity> GetMock<TEntity>(TEntity forItem)
            where TEntity : class =>
            Mock.Get(forItem);
    }
}
