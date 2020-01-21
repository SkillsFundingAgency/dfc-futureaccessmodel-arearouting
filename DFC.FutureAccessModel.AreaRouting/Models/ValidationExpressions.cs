namespace DFC.FutureAccessModel.AreaRouting.Models
{
    /// <summary>
    /// regular expressions used during record validation
    /// </summary>
    public static class ValidationExpressions
    {
        /// <summary>
        /// standard text validation expression
        /// </summary>
        public const string StandardText = @"^[a-zA-Z ]+((['\,\.\- ][a-zA-Z ])?[a-zA-Z ]*)*$";

        /// <summary>
        /// email address validation expression
        /// </summary>
        public const string EmailAddress = @"^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$";

        /// <summary>
        /// phone number validation expression
        /// </summary>
        public const string PhoneNumber = @"^((\\(?0\\d{4}\\)?\\s?\\d{3}\\s?(\\d{3}|\\d{2}))|(\\(?0\\d{3}\\)?\\s?\\d{3}\\s?(\\d{4}|\\d{3}))|(\\(?0\\d{2}\\)?\";

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
