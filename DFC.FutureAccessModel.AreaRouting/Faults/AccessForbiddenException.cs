using System;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// access forbidden exception
    /// </summary>
    public sealed class AccessForbiddenException :
        Exception
    {
        public AccessForbiddenException() :
            base("Insufficient access to this resource")
        {
        }
    }
}
