using Api.Errors.Models;
using Core;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Application.Exceptions
{
    [Serializable]
    public sealed class PayloadSourceNotFoundException : PayloadException
    {
        public PayloadSourceNotFoundException(string payloadFileName)
            : base(payloadFileName)
        {
        }

        public PayloadSourceNotFoundException(string payloadFileName, Exception inner)
            : base($"The given Payload source file {payloadFileName} could not be found", inner)
        {
        }

        [ExcludeFromCodeCoverage]
        private PayloadSourceNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

    }
}
