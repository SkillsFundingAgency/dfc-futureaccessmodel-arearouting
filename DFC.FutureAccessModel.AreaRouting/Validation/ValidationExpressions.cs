namespace DFC.FutureAccessModel.AreaRouting.Validation
{
    /// <summary>
    /// regular expressions used during record validation
    /// </summary>
    public static class ValidationExpressions
    {
        /// <summary>
        /// touchpoint identifier expression
        /// </summary>
        public const string TouchpointID = @"^[0-9]*";

        /// <summary>
        /// town or region validation expression
        /// </summary>
        public const string TownOrRegion = @"^[A-Za-z' \-]*";

        /// <summary>
        /// email address validation expression
        /// </summary>
        public const string EmailAddress = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";

        /// <summary>
        /// phone number validation expression
        /// </summary>
        public const string PhoneNumber = @"^(((\+44\s?\d{3,4}|\(?0\d{3,4}\)?)\s?\d{3,4}\s?\d{3,4})|((\+44\s?\d{3,4}|\(?0\d{3,4}\)?)\s?\d{3,4}\s?\d{3,4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$";

        /// <summary>
        /// mobile number validation expression
        /// </summary>
        public const string MobileNumber = @"^(\+44\s?7\d{3}|\(?07\d{3}\)?)\s?\d{3}\s?\d{3}$";

        /// <summary>
        /// postcode validation expression
        /// </summary>
        public const string Postcode = @"^([A-Za-z][A-Ha-hJ-Yj-y]?[0-9][A-Za-z0-9]? ?[0-9][A-Za-z]{2}|[Gg][Ii][Rr] ?0[Aa]{2})$";

        /// <summary>
        /// postcode outward code validation expression
        /// the outward code is the first part of a postcode
        /// </summary>
        public const string OutwardCode = @"^([A-Za-z][A-Ha-hJ-Yj-y]?[0-9][A-Za-z0-9]?|[Gg][Ii][Rr])$";
    }
}
