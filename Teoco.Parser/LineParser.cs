using Teoco.Interface;

namespace Teoco.Parser
{
    /// <summary>
    /// Used for parsing individual line and and getting back LineModel 
    /// </summary>
    public class LineParser : ILineParser
    {
        private readonly ITimeParser _timeParser;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timeParser"></param>
        public LineParser(ITimeParser timeParser)
        {
            _timeParser = timeParser;
        }

        /// <summary>
        /// Parse a line
        /// </summary>
        /// <param name="line">String line</param>
        /// <returns>instance of LineModel</returns>
        public LineModel ParseLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                throw new InvalidLineException();
            }
            string[] lineItems = line.Split(',');
            if (lineItems.Length != 3)
            {
                throw new InvalidLineException(
                    "Line data is not valid. Line data should be of format: timespan, " +
                    $"dataset, value. Where as input line was {line}");
            }

            var lineModel = new LineModel();
            lineModel.TimeSpan = _timeParser.ParseTime(lineItems[0]);

            lineModel.DataSet = ParseData(lineItems[1], "DataSet");
            lineModel.Value = ParseData(lineItems[2], "Value");

            return lineModel;
        }

        /// <summary>
        /// Parse DataSet and Value
        /// </summary>
        /// <param name="data">string data</param>
        /// <param name="key">DataSet or Value</param>
        /// <returns>int value</returns>
        private int ParseData(string data, string key)
        {
            int value = 0;
            int.TryParse(data, out value);
            if (value == 0)
            {
                throw new InvalidLineException($"Line data is not valid. {key} should be greater than 0");
            }
            return value;
        }
    }
}
