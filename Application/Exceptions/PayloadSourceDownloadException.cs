using Api.Errors.Models;
using Core;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Application.Exceptions
{
    [Serializable]
    public sealed class PayloadSourceDownloadException : PayloadException
    {
        public PayloadSourceDownloadException(string message, string fileName)
            : base($"{message} '{fileName}'")
        {
        }

        public PayloadSourceDownloadException(string message, string fileName, Exception innerException)
            : base($"{message} '{fileName}'", innerException)
        {
        }

        [ExcludeFromCodeCoverage]
        private PayloadSourceDownloadException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

    }
}
