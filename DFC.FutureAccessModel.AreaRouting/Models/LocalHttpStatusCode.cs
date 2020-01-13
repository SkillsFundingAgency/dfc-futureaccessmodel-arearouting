namespace DFC.FutureAccessModel.AreaRouting.Models
{
    /// <summary>
    /// local http status codes
    /// </summary>
    public enum LocalHttpStatusCode
    {
        /// <summary>
        /// an unprocessable entity
        /// </summary>
        UnprocessableEntity = 422,

        // TODO: how is this to be processed? what will it 'map' too?
        /// <summary>
        /// too many requests (returned in a document client exception)
        /// </summary>
        TooManyRequests = 429
    }
}
