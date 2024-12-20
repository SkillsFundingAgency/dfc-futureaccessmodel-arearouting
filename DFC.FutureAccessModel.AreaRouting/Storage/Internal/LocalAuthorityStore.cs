using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Wrappers;
using System;
using System.Threading.Tasks;

namespace DFC.FutureAccessModel.AreaRouting.Storage.Internal
{
    /// <summary>
    /// the area routing detail store
    /// </summary>
    internal sealed class LocalAuthorityStore :
        IStoreLocalAuthorities
    {
        /// <summary>
        /// the cosmos db wrapper
        /// </summary>
        public IWrapCosmosDbClient CosmosDbWrapper { get; }

        /// <summary>
        /// create an instance of hte <see cref="LocalAuthorityStore"/>
        /// </summary>        
        public LocalAuthorityStore(
            IWrapCosmosDbClient cosmosDbWrapper)
        {
            It.IsNull(cosmosDbWrapper)
                .AsGuard<ArgumentNullException>(nameof(cosmosDbWrapper));

            CosmosDbWrapper = cosmosDbWrapper;
        }

        /// <summary>
        /// get (the) local authority for...
        /// </summary>
        /// <param name="theAdminDistrict">the admin distict (code)</param>
        /// <returns>a local authority</returns>
        public async Task<ILocalAuthority> Get(string theAdminDistrict)
        {
            return await CosmosDbWrapper.GetLocalAuthorityAsync(theAdminDistrict);
        }
    }
}