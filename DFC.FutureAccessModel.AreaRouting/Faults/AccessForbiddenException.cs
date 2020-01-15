using System;
using System.Runtime.Serialization;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// access forbidden exception
    /// contructors and decorators are here to satisfy the static analysis tool
    /// </summary>
    [Serializable]
    public class AccessForbiddenException :
        Exception
    {
        /// <summary>
        /// the exception message
        /// </summary>
        /// <returns>the exception message</returns>
        public const string ExceptionMessage = "Insufficient access to this resource";

        /// <summary>
        /// initialises an instance of the <see cref="AccessForbiddenException"/>
        /// </summary>
        public AccessForbiddenException() :
            this(ExceptionMessage)
        {
        }

        /// <summary>
        /// initialises an instance of the <see cref="AccessForbiddenException"/>
        /// </summary>
        /// <param name="message">message</param>
        public AccessForbiddenException(string message) :
            base(message)
        {
        }

        /// <summary>
        /// initialises an instance of the <see cref="AccessForbiddenException"/>
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="innerException">inner exception</param>
        public AccessForbiddenException(string message, Exception innerException) :
            base(message, innerException)
        {
        }

        /// <summary>
        /// initialises an instance of the <see cref="AccessForbiddenException"/>
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">context</param>
        protected AccessForbiddenException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }
    }
}
