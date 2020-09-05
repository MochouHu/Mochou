using System;
using System.Runtime.Serialization;

namespace Mochou.Core.AOP
{
    [Serializable]
    internal class AOPException : Exception
    {
        public AOPException()
        {
        }

        public AOPException(string message) : base(message)
        {
        }

        public AOPException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AOPException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}