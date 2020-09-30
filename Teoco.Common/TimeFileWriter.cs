using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Teoco.Interface;

namespace Teoco.Common
{
    /// <summary>
    /// Write the time file in the given order
    /// </summary>
    public class TimeFileWriter : ITimeFileWriter
    {
        private readonly ITimeParser _timeParser;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timeParser">Implementation of ITimeParser</param>
        public TimeFileWriter(ITimeParser timeParser)
        {
            _timeParser = timeParser;
        }

        /// <summary>
        /// Writes the file in the given path with given order
        /// </summary>
        /// <param name="filePath">Path to write the file</param>
        /// <param name="orderedQuery">Order query</param>
        /// <returns>Task</returns>
        public virtual async Task WriteToFile(string filePath, IOrderedEnumerable<LineModel> orderedQuery)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    await streamWriter.WriteLineAsync("timespan, dataset, value");
                    foreach (var l in orderedQuery)
                    {
                        await streamWriter.WriteLineAsync($"{_timeParser.ToTimeString(l.TimeSpan)}, {l.DataSet}, {l.Value}");
                    };
                }
            }
        }
    }
}
