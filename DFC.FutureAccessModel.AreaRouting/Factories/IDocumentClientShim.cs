using System;
using System.Threading.Tasks;

namespace DFC.FutureAccessModel.AreaRouting.Factories
{
    public interface IDocumentClientShim
    {
        Task<TResource> CreateDocumentAsync<TResource>(Uri documentCollectionUri, TResource document)
            where TResource : class;

        Task<TResource> ReadDocumentAsync<TResource>(Uri documentUri)
            where TResource : class;
    }
}
