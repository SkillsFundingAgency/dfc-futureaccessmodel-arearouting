using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// the malformed request exception
    /// constructors and decorators are here to satisfy the static analysis tool
    /// as a consequence, excluded from coverage as they can't be tested properly
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class MalformedRequestException :
        Exception
    {
        /// <summary>
        /// initialises an instance of the <see cref="MalformedRequestException"/>
        /// </summary>
        public MalformedRequestException() :
            this(string.Empty)
        {
        }

        /// <summary>
        /// initialises an instance of the <see cref="MalformedRequestException"/>
        /// </summary>
        /// <param name="message">message</param>
        public MalformedRequestException(string message) :
            base(message)
        {
        }

        /// <summary>
        /// initialises an instance of the <see cref="MalformedRequestException"/>
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="innerException">inner exception</param>
        public MalformedRequestException(string message, Exception innerException) :
            base(message, innerException)
        {
        }

        /// <summary>
        /// initialises an instance of the <see cref="MalformedRequestException"/>
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">context</param>
        protected MalformedRequestException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }
    }
}
