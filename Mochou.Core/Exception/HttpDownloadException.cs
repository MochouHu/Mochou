using System;
using System.Runtime.Serialization;

namespace Mochou.Core
{
    [Serializable]
    internal class HttpDownloadException : Exception
    {
        private Exception e;

        public HttpDownloadException()
        {
        }

        public HttpDownloadException(Exception e)
        {
            this.e = e;
        }

        public HttpDownloadException(string message) : base(message)
        {
        }

        public HttpDownloadException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpDownloadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}