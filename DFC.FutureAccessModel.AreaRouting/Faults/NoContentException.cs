using System;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// no content exception
    /// </summary>
    public sealed class NoContentException :
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

        public static string GetMessage(string parentResourceName = null) =>
            parentResourceName != null
                ? $"'{parentResourceName}' does not exist"
                : "Resource does not exist";
    }
}
