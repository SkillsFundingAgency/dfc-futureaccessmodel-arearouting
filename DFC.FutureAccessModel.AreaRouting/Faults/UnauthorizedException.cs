using System;
using System.Runtime.Serialization;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// the unauthorised exception
    /// contructors and decorators are here to satisfy the static analysis tool
    /// </summary>
    [Serializable]
    public class UnauthorizedException :
        Exception
    {
        /// <summary>
        /// initialises an instance of the <see cref="UnauthorizedAccessException"/>
        /// </summary>
        public UnauthorizedException()
        {
        }

        /// <summary>
        /// initialises an instance of the <see cref="UnauthorizedAccessException"/>
        /// </summary>
        /// <param name="message">message</param>
        public UnauthorizedException(string message) :
            base(message)
        {
        }

        /// <summary>
        /// initialises an instance of the <see cref="UnauthorizedAccessException"/>
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="innerException">inner exception</param>
        public UnauthorizedException(string message, Exception innerException) :
            base(message, innerException)
        {
        }

        /// <summary>
        /// initialises an instance of the <see cref="UnauthorizedAccessException"/>
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">context</param>
        protected UnauthorizedException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }
    }
}
