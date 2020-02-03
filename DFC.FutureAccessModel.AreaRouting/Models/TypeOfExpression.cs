namespace DFC.FutureAccessModel.AreaRouting.Models
{
    // the type of (request) expression
    public enum TypeOfExpression
    {
        Unknown,

        /// <summary>
        /// town
        /// </summary>
        Town,

        /// <summary>
        /// outward part of the postcode
        /// </summary>
        Outward,

        /// <summary>
        /// postscode
        /// </summary>
        Postcode,
    }
}
