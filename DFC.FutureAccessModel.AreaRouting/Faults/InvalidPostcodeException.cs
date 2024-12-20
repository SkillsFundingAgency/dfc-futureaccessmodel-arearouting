using DFC.FutureAccessModel.AreaRouting.Helpers;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// the invalid postcode exception
    /// </summary>    
    [ExcludeFromCodeCoverage]
    public class InvalidPostcodeException :
        Exception
    {
        /// <summary>
        /// get message...
        /// </summary>
        /// <param name="forTheSubmittedValue">for hte submitted value</param>
        /// <returns></returns>
        public static string GetMessage(string forTheSubmittedValue = null) =>
            It.Has(forTheSubmittedValue)
                ? $"Invalid postcode submitted: '{forTheSubmittedValue}'"
                : "Invalid postcode submitted";

        /// <summary>
        /// instaniates a new instance of <see cref="InvalidPostcodeException"/>
        /// </summary>
        public InvalidPostcodeException() :
            base(GetMessage())
        {
        }

        /// <summary>
        /// instaniates a new instance of <see cref="InvalidPostcodeException"/>
        /// </summary>
        /// <param name="message">message</param>
        public InvalidPostcodeException(string message) :
            base(GetMessage(message))
        {
        }

        /// <summary>
        /// instaniates a new instance of <see cref="InvalidPostcodeException"/>
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="innerException">inner exception</param>
        public InvalidPostcodeException(string message, Exception innerException) :
            base(GetMessage(message), innerException)
        {
        }
    }
}
