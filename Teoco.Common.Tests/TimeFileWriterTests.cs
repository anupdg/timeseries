using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Teoco.Interface;
using System.Linq;

namespace Teoco.Common.Tests
{
    [TestClass]
    public class TimeFileWriterTests
    {
        private string fileToWrite;
        private List<LineModel> lineModels;

        Mock<IConfiguration> config;

        [TestInitialize]
        public void Config()
        {
            fileToWrite = $"{Directory.GetCurrentDirectory()}/{Path.DirectorySeparatorChar}/Temp_File.txt";

            lineModels = new List<LineModel>() {
                new LineModel(){ TimeSpan = new TimeModel(){ Years= 6, Months=2, Weeks=3, Days = 4, Minutes= 5 }, DataSet=10, Value=22 },
                new LineModel(){ TimeSpan = new TimeModel(){ Years= 1, Months=7, Weeks=8, Days = 9, Minutes= 10 }, DataSet=1, Value=22 }
            };
        }

        [TestMethod]
        public async Task ToTimeString_Called_N_Times()
        {
            //Setup
            var parser = new Mock<ITimeParser>();
            parser.Setup(p => p.ToTimeString(It.IsAny<TimeModel>())).Returns(It.IsAny<string>());
            int times = lineModels.Count();

            //Operation
            ITimeFileWriter reader = new TimeFileWriter(parser.Object);
            await reader.WriteToFile(fileToWrite, lineModels.OrderBy(c => c.DataSet));

            //Assertions
            parser.Verify(p => p.ToTimeString(It.IsAny<TimeModel>()), Times.Exactly(times));
        }

        [TestMethod]
        public async Task ToTimeString_Called_0_Times()
        {
            //Setup
            var parser = new Mock<ITimeParser>();
            parser.Setup(p => p.ToTimeString(It.IsAny<TimeModel>())).Returns(It.IsAny<string>());
            lineModels.Clear();

            //Operation
            ITimeFileWriter reader = new TimeFileWriter(parser.Object);
            await reader.WriteToFile(fileToWrite, lineModels.OrderBy(c => c.DataSet));

            //Assertions
            parser.Verify(p => p.ToTimeString(It.IsAny<TimeModel>()), Times.Never());
        }

        [TestMethod]
        public async Task WriteToFile_File_Got_Created()
        {
            //Setup
            var parser = new Mock<ITimeParser>();
            parser.Setup(p => p.ToTimeString(It.IsAny<TimeModel>())).Returns(It.IsAny<string>());

            //Operation
            ITimeFileWriter reader = new TimeFileWriter(parser.Object);
            await reader.WriteToFile(fileToWrite, lineModels.OrderBy(c => c.DataSet));

            //Assertions
            bool fileExists = File.Exists(fileToWrite);
            Assert.AreEqual(fileExists, true);
        }

        [TestMethod]
        public async Task WriteToFile_Created_File_Line_Count()
        {
            //Setup
            var parser = new Mock<ITimeParser>();
            parser.Setup(p => p.ToTimeString(It.IsAny<TimeModel>())).Returns(It.IsAny<string>());

            //Operation
            ITimeFileWriter reader = new TimeFileWriter(parser.Object);
            await reader.WriteToFile(fileToWrite, lineModels.OrderBy(c => c.DataSet));

            //Assertions
            var lines = File.ReadAllLines(fileToWrite);
            Assert.AreEqual(lines.Length, lineModels.Count() + 1);
        }

        [TestCleanup]
        public void Clear()
        {
            if (File.Exists(fileToWrite))
            {
                File.Delete(fileToWrite);
            }
        }

    }
}
