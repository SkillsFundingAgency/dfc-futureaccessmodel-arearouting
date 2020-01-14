using System;
using System.Runtime.Serialization;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// no content exception
    /// </summary>
    [Serializable]
    public class NoContentException :
            Exception
    {
        public NoContentException() :
            base(GetMessage())
        {
        }

        public NoContentException(string parentResource) :
            base(GetMessage(parentResource))
        {
        }

        public NoContentException(string message, Exception innerException) :
            base(message, innerException)
        {
        }

        protected NoContentException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }

        public static string GetMessage(string parentResourceName = null) =>
            parentResourceName != null
                ? $"'{parentResourceName}' does not exist"
                : "Resource does not exist";
    }
}
