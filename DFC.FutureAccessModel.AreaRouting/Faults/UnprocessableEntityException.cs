using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// unprocessable entity exception
    /// </summary>
    public sealed class UnprocessableEntityException :
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

        public static string GetMessage(IReadOnlyDictionary<string, string> errors = null)
        {
            var localErrs = errors ?? new Dictionary<string, string>();
            return $"{{ \"errors\": [{{ {string.Join(",", localErrs.Select(x => $"\"error\": {{ \"code\": \"{x.Key}, \"message\": \"{x.Value}\" }}"))} }}] }}";
        }
    }
}
