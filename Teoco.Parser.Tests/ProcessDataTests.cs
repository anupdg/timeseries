using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teoco.Common;
using Teoco.Interface;

namespace Teoco.Parser.Tests
{
    [TestClass]
    public class ProcessDataTests
    {
        Mock<IConfiguration> config;
        Mock<ITimeParser> timeParser;
        Mock<LineParser> lineParser;
        Mock<TimeFileReader> timeFileReader;
        Mock<TimeFileWriter> timeFileWriter;
        DateTime time;
        [TestInitialize]
        public void Config()
        {
            config = new Mock<IConfiguration>();

            var configurationSection = new Mock<IConfigurationSection>();
            configurationSection.Setup(a => a.Value).Returns("test-data.txt");

            config.Setup(a => a.GetSection("FileToParse")).Returns(configurationSection.Object);

            timeParser = new Mock<ITimeParser>();
            lineParser = new Mock<LineParser>(timeParser.Object);
            timeFileReader = new Mock<TimeFileReader>(config.Object, lineParser.Object);
            timeFileWriter = new Mock<TimeFileWriter>(timeParser.Object);
            time = DateTime.Now;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException), "Process_TimeData_Null")]
        public async Task Process_TimeData_Null()
        {
            //Setup
            List<LineModel> lineModels = null;
            timeFileReader.Setup(p => p.ReadFile()).Returns(Task.FromResult(lineModels));

            //Operation
            IProcessData processData = new ProcessData(timeFileReader.Object, timeFileWriter.Object);
            var data = await processData.Process(time);

            //Assertions
            //Exception assertion
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException), "Process_TimeData_Count_0")]
        public async Task Process_TimeData_Count_0()
        {
            //Setup
            List<LineModel> lineModels = new List<LineModel>();
            timeFileReader.Setup(p => p.ReadFile()).Returns(Task.FromResult(lineModels));


            //Operation
            IProcessData processData = new ProcessData(timeFileReader.Object, timeFileWriter.Object);
            var data = await processData.Process(time);

            //Assertions
            //Exception assertion
        }

        [TestMethod]
        public async Task GetByTimePath_CheckPath()
        {
            //Setup
            List<LineModel> lineModels = new List<LineModel>() { new LineModel() };
            timeFileReader.Setup(p => p.ReadFile()).Returns(Task.FromResult(lineModels));
            timeFileWriter.Setup(p => p.WriteToFile(It.IsAny<string>(), It.IsAny<IOrderedEnumerable<LineModel>>()));

            //Operation
            IProcessData processData = new ProcessData(timeFileReader.Object, timeFileWriter.Object);
            var data = await processData.Process(time);

            //Assertions
            Assert.AreEqual(data.Item1, $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}ByTime{time.ToString("yyyyMMddhhmm")}.txt");
        }

        [TestMethod]
        public async Task GetByDataSetPath_CheckPath()
        {
            //Setup
            List<LineModel> lineModels = new List<LineModel>() { new LineModel() };
            timeFileReader.Setup(p => p.ReadFile()).Returns(Task.FromResult(lineModels));
            timeFileWriter.Setup(p => p.WriteToFile(It.IsAny<string>(), It.IsAny<IOrderedEnumerable<LineModel>>()));

            //Operation
            IProcessData processData = new ProcessData(timeFileReader.Object, timeFileWriter.Object);
            var data = await processData.Process(time);

            //Assertions
            Assert.AreEqual(data.Item2, $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}ByDataSet{time.ToString("yyyyMMddhhmm")}.txt");
        }

        [TestMethod]
        public void SortByTime_CheckOrder()
        {
            //Setup
            var timeData = new List<LineModel> {
                new LineModel() { TimeSpan = new TimeModel() { Years = 2 }, DataSet = 3, Value = 5 },
                new LineModel() { TimeSpan = new TimeModel() { Years = 1 }, DataSet = 5, Value = 2 }
            };

            //Operation
            IProcessData processData = new ProcessData(timeFileReader.Object, timeFileWriter.Object);
            var data = processData.SortByTime(timeData).ToList();

            //Assertions
            Assert.AreEqual(data.Count, 2);
            Assert.AreEqual(timeData[1].TimeSpan.Years, 1);
            Assert.AreEqual(data[0].TimeSpan.Years, 1);
        }

        [TestMethod]
        public void SortByDataSet_CheckOrder()
        {
            //Setup
            var timeData = new List<LineModel> {
                new LineModel() { TimeSpan = new TimeModel() { Years = 2 }, DataSet = 3, Value = 5 },
                new LineModel() { TimeSpan = new TimeModel() { Years = 1 }, DataSet = 2, Value = 2 }
            };

            //Operation
            IProcessData processData = new ProcessData(timeFileReader.Object, timeFileWriter.Object);
            var data = processData.SortByDataSet(timeData).ToList();

            //Assertions
            Assert.AreEqual(data.Count, 2);
            Assert.AreEqual(timeData[1].DataSet, 2);
            Assert.AreEqual(data[0].DataSet, 2);
        }

        [TestMethod]
        public async Task Process_WriteToFile_Called()
        {
            //Setup
            List<LineModel> lineModels = new List<LineModel> {
                new LineModel() { TimeSpan = new TimeModel() { Years = 2 }, DataSet = 3, Value = 5 },
                new LineModel() { TimeSpan = new TimeModel() { Years = 1 }, DataSet = 2, Value = 2 }
            };
            timeFileReader.Setup(p => p.ReadFile()).Returns(Task.FromResult(lineModels));
            timeFileWriter.Setup(p => p.WriteToFile(It.IsAny<string>(), It.IsAny<IOrderedEnumerable<LineModel>>()));

            //Operation
            IProcessData processData = new ProcessData(timeFileReader.Object, timeFileWriter.Object);
            var data = await processData.Process(time);

            //Assertions
            timeFileWriter.Verify(p => p.WriteToFile(It.IsAny<string>(), It.IsAny<IOrderedEnumerable<LineModel>>()), Times.Exactly(2));
        }
    }
}
