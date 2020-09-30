using System;

namespace Teoco.Parser
{
    /// <summary>
    /// InvalidLineException class
    /// </summary>
    public class InvalidLineException : Exception
    {
        public InvalidLineException() : base("Line data is not valid") { }
        public InvalidLineException(string lineString) : base($"Line data is not valid: {lineString}") { }
    }
}
