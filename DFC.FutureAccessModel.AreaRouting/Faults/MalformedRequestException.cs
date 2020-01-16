﻿using System;
using System.Runtime.Serialization;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// the malformed request exception
    /// contructors and decorators are here to satisfy the static analysis tool
    /// </summary>
    [Serializable]
    public class MalformedRequestException :
        Exception
    {
        /// <summary>
        /// initialises an instance of the <see cref="MalformedRequestException"/>
        /// </summary>
        public MalformedRequestException() :
            base(string.Empty)
        {
        }

        /// <summary>
        /// initialises an instance of the <see cref="MalformedRequestException"/>
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">context</param>
        protected MalformedRequestException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }
    }
}
