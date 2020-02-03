﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace DFC.FutureAccessModel.AreaRouting.Faults
{
    /// <summary>
    /// the malformed request exception
    /// constructors and decorators are here to satisfy the static analysis tool
    /// as a consequence, excluded from coverage as they can't be tested properly
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class MalformedRequestException :
        Exception
    {
        /// <summary>
        /// the exception message
        /// </summary>
        public const string ExceptionMessage = "";

        /// <summary>
        /// initialises an instance of the <see cref="MalformedRequestException"/>
        /// </summary>
        public MalformedRequestException() :
            this(ExceptionMessage)
        {
        }

        /// <summary>
        /// initialises an instance of the <see cref="MalformedRequestException"/>
        /// </summary>
        /// <param name="message">message</param>
        public MalformedRequestException(string message) :
            base(ExceptionMessage)
        {
        }

        /// <summary>
        /// initialises an instance of the <see cref="MalformedRequestException"/>
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="innerException">inner exception</param>
        public MalformedRequestException(string message, Exception innerException) :
            base(ExceptionMessage, innerException)
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
