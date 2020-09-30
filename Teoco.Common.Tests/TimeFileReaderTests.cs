using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Threading.Tasks;
using Teoco.Interface;

namespace Teoco.Common.Tests
{
    [TestClass]
    public class TimeFileReaderTests
    {
        Mock<IConfiguration> config;

        [TestInitialize]
        public void Config()
        {
            config = new Mock<IConfiguration>();

            var configurationSection = new Mock<IConfigurationSection>();
            configurationSection.Setup(a => a.Value).Returns("test-data.txt");

            config.Setup(a => a.GetSection("FileToParse")).Returns(configurationSection.Object);
        }

        [TestMethod]
        public async Task ParseLine_Called()
        {
            //Setup
            var parser = new Mock<ILineParser>();
            parser.Setup(p => p.ParseLine(It.IsAny<string>())).Returns(new LineModel());

            //Operation
            ITimeFileReader reader = new TimeFileReader(config.Object, parser.Object);
            await reader.ReadFile();

            //Assertions
            parser.Verify(p => p.ParseLine(It.IsAny<string>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public async Task ParseLine_Called_Times()
        {
            //Setup
            var parser = new Mock<ILineParser>();
            parser.Setup(p => p.ParseLine(It.IsAny<string>())).Returns(It.IsAny<LineModel>());

            //Operation
            ITimeFileReader reader = new TimeFileReader(config.Object, parser.Object);
            var result = await reader.ReadFile();

            //Assertions
            Assert.AreEqual(result.Count, 7);
            //Question: Why 7? test-data.txt file has 7 records.
            //Question: Why Actual file and not mock? We are using FileStream, BufferedStream, StreamReader for reading file. 
            //No point is mocking them
        }

        [TestMethod]
        public async Task ReadFile_CheckValues()
        {
            //Setup
            var line = new LineModel()
            {
                TimeSpan = new TimeModel()
                {
                    Years = 1,
                    Months = 6,
                    Weeks = 1,
                    Days = 2,
                    Minutes = 3
                },
                DataSet = 12,
                Value = 2150
            };
            var parser = new Mock<ILineParser>();
            parser.Setup(p => p.ParseLine(It.IsAny<string>())).Returns(line);

            //Operation
            ITimeFileReader reader = new TimeFileReader(config.Object, parser.Object);
            var result = await reader.ReadFile();

            //Assertions
            Assert.AreEqual(result[0].DataSet, 12);
            Assert.AreEqual(result[0].Value, 2150);
            Assert.AreEqual(result[0].TimeSpan.Years, 1);
            Assert.AreEqual(result[0].TimeSpan.Months, 6);
            Assert.AreEqual(result[0].TimeSpan.Weeks, 1);
            Assert.AreEqual(result[0].TimeSpan.Days, 2);
            Assert.AreEqual(result[0].TimeSpan.Minutes, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException), "ReadFile_NotExists: File does not exists")]
        public async Task ReadFile_NotExists()
        {
            //Setup
            var parser = new Mock<ILineParser>();
            var configTemp = new Mock<IConfiguration>();

            var configurationSection = new Mock<IConfigurationSection>();
            configurationSection.Setup(a => a.Value).Returns("file-does-not-exits.txt");

            configTemp.Setup(a => a.GetSection("FileToParse")).Returns(configurationSection.Object);

            //Operation
            ITimeFileReader reader = new TimeFileReader(configTemp.Object, parser.Object);
            var result = await reader.ReadFile();

            //Assertions
            //Exception assertion
        }
    }
}
