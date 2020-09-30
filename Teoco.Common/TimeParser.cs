using System.Text;
using System.Text.RegularExpressions;
using Teoco.Interface;

namespace Teoco.Common
{
    /// <summary>
    /// Used for parsing time data and 
    /// Converting time mode to time string
    /// </summary>
    public class TimeParser : ITimeParser
    {
        private StringBuilder builder;

        /// <summary>
        /// Constructor
        /// </summary>
        public TimeParser()
        {
            builder = new StringBuilder();
        }

        /// <summary>
        /// This function takes input as ISO_8601 Duration format (P[n]Y[n]M[n]W[n]D[n]H[n]M[n]S) and returns a custom time Class
        /// Current limitation of this is it extract Y[n]M[n]W[n]D[n] part of ISO_8601 Duration specification
        /// </summary>
        /// <param name="timeStr">ISO_8601 Duration substring</param>
        /// <returns>Instance of TimeModel</returns>
        public TimeModel ParseTime(string timeStr)
        {
            var m = Regex.Match(timeStr, @"^((?<years>\d+)y)?((?<months>\d+)m)?((?<weeks>\d+)w)?((?<days>\d+)d)?((?<minutes>\d+)m)?$", RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.RightToLeft);
            int yr = m.Groups["years"].Success ? int.Parse(m.Groups["years"].Value) : 0;
            int mt = m.Groups["months"].Success ? int.Parse(m.Groups["months"].Value) : 0;
            int wk = m.Groups["weeks"].Success ? int.Parse(m.Groups["weeks"].Value) : 0;
            int d = m.Groups["days"].Success ? int.Parse(m.Groups["days"].Value) : 0;
            int min = m.Groups["minutes"].Success ? int.Parse(m.Groups["minutes"].Value) : 0;

            if (yr == 0 && mt == 0 && wk == 0 && d == 0 && min == 0)
            {
                throw new InvalidTimeException(timeStr);
            }
            return new TimeModel() { Years = yr, Months = mt, Weeks = wk, Days = d, Minutes = min };
        }

        /// <summary>
        ///  Returns ISO_8601 Duration format ([n]y[n]m[n]w[n]d[n]h[n]m[n]s) 
        /// </summary>
        /// <param name="timeModel"></param>
        /// <returns></returns>
        public string ToTimeString(TimeModel timeModel)
        {
            if (timeModel == null)
            {
                throw new InvalidTimeException("TimeModel timeModel");
            }
            builder.Clear();
            if (timeModel.Years > 0)
            {
                builder.Append($"{timeModel.Years}y");
            }
            if (timeModel.Months > 0)
            {
                builder.Append($"{timeModel.Months}m");
            }
            if (timeModel.Weeks > 0)
            {
                builder.Append($"{timeModel.Weeks}w");
            }
            if (timeModel.Days > 0)
            {
                builder.Append($"{timeModel.Days}d");
            }
            if (timeModel.Minutes > 0)
            {
                builder.Append($"{timeModel.Minutes}m");
            }
            string time = builder.ToString();
            if (string.IsNullOrWhiteSpace(time))
            {
                throw new InvalidTimeException("TimeModel timeModel");
            }
            else
            {
                return time;
            }
        }
    }
}
