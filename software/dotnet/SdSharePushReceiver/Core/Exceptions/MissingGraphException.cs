using System;

namespace SdShare.Exceptions
{
    public class MissingGraphException : Exception
    {
        public MissingGraphException() { }

        public MissingGraphException(string message) : base(message) { }

        public MissingGraphException(string message, Exception inner) : base(message, inner) { }
    }
}
