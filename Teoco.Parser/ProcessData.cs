using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Teoco.Interface;

namespace Teoco.Parser
{
    /// <summary>
    /// Main class used for processing and saving the data in two different file
    /// </summary>
    public class ProcessData : IProcessData
    {
        private readonly ITimeFileReader _timeFileReader;
        private readonly ITimeFileWriter _timeFileWriter;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timeFileReader">Implementation of ITimeFileReader</param>
        /// <param name="timeFileWriter">Implementation of ITimeFileWriter</param>
        public ProcessData(ITimeFileReader timeFileReader, ITimeFileWriter timeFileWriter)
        {
            _timeFileReader = timeFileReader;
            _timeFileWriter = timeFileWriter;
        }

        /// <summary>
        /// Get file path for sort by time
        /// </summary>
        /// <param name="processTime">timestamp</param>
        /// <returns>Path</returns>
        public string GetByTimePath(DateTime processTime)
        {
            return $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}ByTime{processTime.ToString("yyyyMMddhhmm")}.txt";
        }

        /// <summary>
        /// Get file path for sort by DataSet
        /// </summary>
        /// <param name="processTime">timestamp</param>
        /// <returns>Path</returns>
        public string GetByDataSetPath(DateTime processTime)
        {
            return $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}ByDataSet{processTime.ToString("yyyyMMddhhmm")}.txt";
        }

        /// <summary>
        /// Returns List of LineModel order by Time
        /// </summary>
        /// <param name="timeData">Input LineModel</param>
        /// <returns>Ordered LineModel</returns>
        public IOrderedEnumerable<LineModel> SortByTime(List<LineModel> timeData)
        {
            return timeData.OrderBy(c => c.TimeSpan.Years)
               .ThenBy(c => c.TimeSpan.Months)
               .ThenBy(c => c.TimeSpan.Weeks)
               .ThenBy(c => c.TimeSpan.Days)
               .ThenBy(c => c.TimeSpan.Minutes)
               .ThenBy(c => c.DataSet)
               .ThenBy(c => c.Value);
        }

        /// <summary>
        /// Returns List of LineModel order by DataSet
        /// </summary>
        /// <param name="timeData">Input LineModel</param>
        /// <returns>Ordered LineModel</returns>
        public IOrderedEnumerable<LineModel> SortByDataSet(List<LineModel> timeData)
        {
            return timeData.OrderBy(c => c.DataSet)
                .ThenBy(c => c.TimeSpan.Years)
                .ThenBy(c => c.TimeSpan.Months)
                .ThenBy(c => c.TimeSpan.Weeks)
                .ThenBy(c => c.TimeSpan.Days)
                .ThenBy(c => c.TimeSpan.Minutes)
                .ThenBy(c => c.Value);
        }

        /// <summary>
        /// Process input file and create two different files with-
        /// Time data order by Time
        /// Time data order by DataSet
        /// </summary>
        /// <param name="processTime">timestamp</param>
        /// <returns>Created files</returns>
        public async Task<Tuple<string, string>> Process(DateTime processTime)
        {
            List<LineModel> timeData = await _timeFileReader.ReadFile();
            if (timeData == null || timeData.Count() == 0)
            {
                throw new InvalidDataException("There is no data to process");
            }
            var getByTimePath = GetByTimePath(processTime);
            var getByDataSetPath = GetByDataSetPath(processTime);

            var byTime = _timeFileWriter.WriteToFile(getByTimePath, SortByTime(timeData));

            var byDataSet = _timeFileWriter.WriteToFile(getByDataSetPath, SortByDataSet(timeData));

            Task.WaitAll(byTime, byDataSet);

            return new Tuple<string, string>(getByTimePath, getByDataSetPath);
        }
    }
}
