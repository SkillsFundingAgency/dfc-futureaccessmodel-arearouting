using System;
using System.Runtime.Serialization;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// no content exception
    /// contructors and decorators are here to satisfy the static analysis tool
    /// </summary>
    [Serializable]
    public class NoContentException :
            Exception
    {
        /// <summary>
        /// initialises an instance of the <see cref="NoContentException"/>
        /// </summary>
        public NoContentException() :
            base(GetMessage())
        {
        }

        /// <summary>
        /// initialises an instance of the <see cref="NoContentException"/>
        /// </summary>
        /// <param name="parentResource">parent resource name</param>
        public NoContentException(string parentResource) :
            base(GetMessage(parentResource))
        {
        }

        /// <summary>
        /// initialises an instance of the <see cref="NoContentException"/>
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="innerException">inner exception</param>
        public NoContentException(string message, Exception innerException) :
            base(message, innerException)
        {
        }

        /// <summary>
        /// initialises an instance of the <see cref="NoContentException"/>
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">context</param>
        protected NoContentException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }

        /// <summary>
        /// get message
        /// </summary>
        /// <param name="parentResourceName">the parent resource name</param>
        /// <returns>the exception message</returns>
        public static string GetMessage(string parentResourceName = null) =>
            parentResourceName != null
                ? $"'{parentResourceName}' does not exist"
                : "Resource does not exist";
    }
}
