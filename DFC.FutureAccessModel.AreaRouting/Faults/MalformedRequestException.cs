using System;
using System.Runtime.Serialization;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    [Serializable]
    public class MalformedRequestException :
        Exception
    {
        public MalformedRequestException()
        {
        }

        public MalformedRequestException(string message) :
            base(message)
        {
        }

        public MalformedRequestException(string message, Exception innerException) :
            base(message, innerException)
        {
        }

        protected MalformedRequestException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }
    }
}
