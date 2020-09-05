using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;

namespace Mochou.Core.Extension
{
    public class ConfigException : Exception
    {
        public ConfigException() : base() { }
        public ConfigException(string message) : base(message) { }
        public ConfigException(string message, Exception innerException):base(message, innerException) { }
        [SecuritySafeCritical]
        protected ConfigException(SerializationInfo info, StreamingContext context):base(info, context) { }
    }
}
