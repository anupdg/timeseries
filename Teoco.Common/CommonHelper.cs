using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Teoco.Common
{
    /// <summary>
    /// Class for common helper methods
    /// </summary>
    public class CommonHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        public static void LogError(ILogger logger, Exception ex, string message) {
            logger.Error(message);
            logger.Error($"Error message: {ex.Message}");
            logger.Error($"Stack: {ex.StackTrace}");
        }
    }
}
