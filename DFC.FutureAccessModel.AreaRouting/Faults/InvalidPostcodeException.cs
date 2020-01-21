using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

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
        /// the exception message
        /// </summary>
        public const string ExceptionMessage = "Invalid postcode submitted";

        /// <summary>
        /// instaniates a new instance of <see cref="InvalidPostcodeException"/>
        /// </summary>
        public InvalidPostcodeException() :
            this(ExceptionMessage)
        {
        }

        /// <summary>
        /// instaniates a new instance of <see cref="InvalidPostcodeException"/>
        /// </summary>
        /// <param name="message">message</param>
        public InvalidPostcodeException(string message) :
            base(ExceptionMessage)
        {
        }

        /// <summary>
        /// instaniates a new instance of <see cref="InvalidPostcodeException"/>
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="innerException">inner exception</param>
        public InvalidPostcodeException(string message, Exception innerException) :
            base(ExceptionMessage, innerException)
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
