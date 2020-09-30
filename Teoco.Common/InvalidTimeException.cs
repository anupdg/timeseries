using System;

namespace Teoco.Common
{
    /// <summary>
    /// InvalidTimeException class
    /// </summary>
    public class InvalidTimeException : Exception
    {
        public InvalidTimeException(string timeString) : base($"Duration string is not valid: {timeString}") { }
    }
}
