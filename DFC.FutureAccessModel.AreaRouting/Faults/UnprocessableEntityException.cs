using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// unprocessable entity exception
    /// </summary>
    [Serializable]
    public class UnprocessableEntityException :
            Exception
    {
        public UnprocessableEntityException() :
            base(GetMessage())
        {
        }

        public UnprocessableEntityException(IReadOnlyDictionary<string, string> errors) :
            base(GetMessage(errors))
        {
        }

        public UnprocessableEntityException(string message) :
            base(message)
        {
        }

        public UnprocessableEntityException(string message, Exception innerException) :
            base(message, innerException)
        {
        }

        protected UnprocessableEntityException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }

        public static string GetMessage(IReadOnlyDictionary<string, string> errors = null)
        {
            var localErrs = errors ?? new Dictionary<string, string>();
            return $"{{ \"errors\": [{{ {string.Join(",", localErrs.Select(x => $"\"error\": {{ \"code\": \"{x.Key}, \"message\": \"{x.Value}\" }}"))} }}] }}";
        }
    }
}
