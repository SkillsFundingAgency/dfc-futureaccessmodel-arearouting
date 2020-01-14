using System;
using System.Runtime.Serialization;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// access forbidden exception
    /// </summary>
    [Serializable]
    public class AccessForbiddenException :
        Exception
    {
        public AccessForbiddenException() :
            base("Insufficient access to this resource")
        {
        }

        public AccessForbiddenException(string message) :
            base(message)
        {
        }

        public AccessForbiddenException(string message, Exception innerException) :
            base(message, innerException)
        {
        }

        protected AccessForbiddenException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }
    }
}
