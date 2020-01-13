using System.Net.Http;

namespace DFC.FutureAccessModel.AreaRouting.Providers.Internal
{
    /// <summary>
    /// the response message helper
    /// </summary>
    public static class ResponseMessageHelper
    {
        /// <summary>
        /// set the content of the response
        /// </summary>
        /// <param name="source">the source (response message)</param>
        /// <param name="newContent">the new content</param>
        /// <returns>the message with new content</returns>
        public static HttpResponseMessage SetContent(this HttpResponseMessage source, string newContent)
        {
            source.Content = new StringContent(newContent);
            return source;
        }
    }
}
