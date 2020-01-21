using Microsoft.Extensions.DependencyInjection;

namespace DFC.FutureAccessModel.AreaRouting.Registration
{
    /// <summary>
    /// i register services
    /// </summary>
    public interface IRegisterServices
    {
        /// <summary>
        /// compose...
        /// </summary>
        /// <param name="usingCollection">using (the) service collection</param>
        void Compose(IServiceCollection usingCollection);
    }
}
