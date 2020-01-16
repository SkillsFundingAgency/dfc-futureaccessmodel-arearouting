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
