using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// the unauthorised exception
    /// constructors and decorators are here to satisfy the static analysis tool
    /// as a consequence, excluded from coverage as they can't be tested properly
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class UnauthorizedException :
        Exception
    {
        /// <summary>
        /// initialises an instance of the <see cref="UnauthorizedException"/>
        /// </summary>
        public UnauthorizedException() :
            base(string.Empty)
        {
        }

        /// <summary>
        /// initialises an instance of the <see cref="UnauthorizedException"/>
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">context</param>
        protected UnauthorizedException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }
    }
}
