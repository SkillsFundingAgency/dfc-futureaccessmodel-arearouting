namespace DFC.FutureAccessModel.AreaRouting.Models
{
    /// <summary>
    /// the type of method will dictate the kind of response we should give
    /// </summary>
    public enum TypeOfFunction
    {
        /// <summary>
        /// get by location
        /// </summary>
        GetByLocation,

        /// <summary>
        /// get by id
        /// </summary>
        GetByID,

        /// <summary>
        /// get all
        /// </summary>
        GetAll,

        /// <summary>
        /// post
        /// </summary>
        Post,

        /// <summary>
        /// delete
        /// </summary>
        Delete
    }
}
