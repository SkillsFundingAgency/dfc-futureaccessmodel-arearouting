using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// access forbidden exception
    /// constructors and decorators are here to satisfy the static analysis tool
    /// as a consequence, excluded from coverage as they can't be tested properly
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
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
            base(ExceptionMessage)
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
