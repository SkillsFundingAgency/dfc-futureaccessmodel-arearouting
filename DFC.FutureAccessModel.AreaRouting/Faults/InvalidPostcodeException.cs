using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using DFC.FutureAccessModel.AreaRouting.Helpers;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// the invalid postcode exception
    /// </summary>
    [Serializable]
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

        /// <summary>
        /// instaniates a new instance of <see cref="InvalidPostcodeException"/>
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">context</param>
        protected InvalidPostcodeException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }
    }
}
