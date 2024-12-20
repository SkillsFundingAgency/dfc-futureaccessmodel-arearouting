using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// the fallback action exception is onlused in the fault response
    /// provider tho detaermine the correct fallback process
    /// for any given method
    /// </summary>    
    [ExcludeFromCodeCoverage]
    public class FallbackActionException :
        Exception
    {
        public FallbackActionException()
        {
        }

        public FallbackActionException(string message) : base(message)
        {
        }

        public FallbackActionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
