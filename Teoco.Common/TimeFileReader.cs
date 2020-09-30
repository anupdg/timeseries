using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Teoco.Interface;

namespace Teoco.Common
{
    /// <summary>
    /// Time file reader, reads the file extract information and returns as list of LineModel
    /// </summary>
    public class TimeFileReader : ITimeFileReader
    {
        private readonly IConfiguration _config;
        private readonly ILineParser _lineParser;
        private string fileToParse;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Implementation of IConfiguration</param>
        /// <param name="lineParser">Implementation of ILineParser</param>
        public TimeFileReader(IConfiguration config, ILineParser lineParser)
        {
            _config = config;
            _lineParser = lineParser;
            GetFilePath();
        }

        /// <summary>
        /// Get the file path from directory using file name(FileToParse) from appsettings file 
        /// </summary>
        /// <returns></returns>
        private string GetFilePath()
        {
            if (string.IsNullOrWhiteSpace(fileToParse))
            {
                fileToParse = _config.GetValue<string>("FileToParse");
                fileToParse = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{fileToParse}";
            }
            return fileToParse;
        }

        /// <summary>
        /// Read file to get the data for parsing
        /// </summary>
        /// <returns>List of LineModel as Task</returns>
        public virtual async Task<List<LineModel>> ReadFile()
        {
            bool isFirstLine = true;
            List<LineModel> timeData = new List<LineModel>();
            using (FileStream fs = File.Open(fileToParse, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    using (StreamReader sr = new StreamReader(bs))
                    {
                        string line;
                        while ((line = await sr.ReadLineAsync()) != null)
                        {
                            if (isFirstLine)
                            {
                                isFirstLine = false;
                                continue;
                            }
                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                var time = _lineParser.ParseLine(line);
                                timeData.Add(time);
                            }
                        }
                    }
                }
            }

            return timeData;
        }
    }
}
