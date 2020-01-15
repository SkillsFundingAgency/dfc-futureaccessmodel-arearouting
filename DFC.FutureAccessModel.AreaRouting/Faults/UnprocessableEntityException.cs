using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// unprocessable entity exception
    /// contructors and decorators are here to satisfy the static analysis tool
    /// </summary>
    [Serializable]
    public class UnprocessableEntityException :
            Exception
    {
        /// <summary>
        /// initialise an instance of the <see cref="UnprocessableEntityException"/>
        /// </summary>
        public UnprocessableEntityException() :
            this(GetMessage())
        {
        }

        /// <summary>
        /// initialise an instance of the <see cref="UnprocessableEntityException"/>
        /// </summary>
        /// <param name="errors"></param>
        public UnprocessableEntityException(IReadOnlyDictionary<string, string> errors) :
            this(GetMessage(errors))
        {
        }

        /// <summary>
        /// initialise an instance of the <see cref="UnprocessableEntityException"/>
        /// </summary>
        /// <param name="message">message</param>
        public UnprocessableEntityException(string message) :
            base(message)
        {
        }

        /// <summary>
        /// initialise an instance of the <see cref="UnprocessableEntityException"/>
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="innerException">inner exception</param>
        public UnprocessableEntityException(string message, Exception innerException) :
            base(message, innerException)
        {
        }

        /// <summary>
        /// initialise an instance of the <see cref="UnprocessableEntityException"/>
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">context</param>
        protected UnprocessableEntityException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }

        /// <summary>
        /// get message transforms the dictionary to a 'json string' of codes and errors
        /// </summary>
        /// <param name="errors">the dictionary of errors</param>
        /// <returns>the message string</returns>
        public static string GetMessage(IReadOnlyDictionary<string, string> errors = null)
        {
            var localErrs = errors ?? new Dictionary<string, string>();
            return $"{{ \"errors\": [{{ {string.Join(",", localErrs.Select(x => $"\"error\": {{ \"code\": \"{x.Key}, \"message\": \"{x.Value}\" }}"))} }}] }}";
        }
    }
}
