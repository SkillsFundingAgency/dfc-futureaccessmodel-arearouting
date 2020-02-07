using System.Net.Http;
using System.Text;
using DFC.FutureAccessModel.AreaRouting.Models;
using Newtonsoft.Json;

namespace DFC.FutureAccessModel.AreaRouting.Helpers
{
    /// <summary>
    /// the response message helper
    /// </summary>
    public static class ResponseMessageHelper
    {
        /// <summary>
        /// the (required) response content type
        /// </summary>
        public const string ResponseContentType = "application/json";

        /// <summary>
        /// the cosmos db's id tag
        /// </summary>
        public const string CosmosDBIDTag = "\"id\"";

        /// <summary>
        ///  the proper object tag
        /// </summary>
        public static readonly string ProperObjectTag = $"\"{nameof(IRoutingDetail.TouchpointID)}\"";

        /// <summary>
        /// set the content of the response
        /// </summary>
        /// <typeparam name="TDocument">wher the <typeparamref name="TDocument"/> is 'I Routing Detail'</typeparam>
        /// <param name="source">the source (response message)</param>
        /// <param name="theContent">the (new) content</param>
        /// <returns>the message with new content</returns>
        public static HttpResponseMessage SetContent<TDocument>(this HttpResponseMessage source, TDocument theContent)
            where TDocument : IRoutingDetail =>
            SetContent(source, JsonConvert.SerializeObject(theContent).Replace(CosmosDBIDTag, ProperObjectTag));

        /// <summary>
        /// set the content of the response
        /// </summary>
        /// <param name="source">the source (response message)</param>
        /// <param name="theContent">the (new) content</param>
        /// <returns>the message with new content</returns>
        public static HttpResponseMessage SetContent(this HttpResponseMessage source, string theContent)
        {
            source.Content = new StringContent(theContent, Encoding.UTF8, ResponseContentType);
            return source;
        }
    }
}
