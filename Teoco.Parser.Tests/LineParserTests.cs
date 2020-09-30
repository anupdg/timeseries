using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Teoco.Common;
using Teoco.Interface;
using Teoco.Parser;

namespace Teoco.Parser.Tests
{
    [TestClass]
    public class LineParserTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidLineException), "ParseLine_InvalidTimeException_Null")]
        public void ParseLine_InvalidLineException_Null()
        {
            //Setup
            var timeParser = new Mock<ITimeParser>();

            //Operation
            ILineParser lineParser = new LineParser(timeParser.Object);
            lineParser.ParseLine(string.Empty);

            //Assertions
            //Exception assertion
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidLineException), "ParseLine_InvalidTimeException_Null")]
        public void ParseLine_InvalidLineException_Invalid()
        {
            //Setup
            var timeParser = new Mock<ITimeParser>();

            //Operation
            ILineParser lineParser = new LineParser(timeParser.Object);
            lineParser.ParseLine("Invalid string");

            //Assertions
            //Exception assertion
        }

        [TestMethod]
        public void ParseLine_InvalidLineException_DataSet_Invalid_data()
        {
            //Setup
            var timeParser = new Mock<ITimeParser>();

            //Operation
            ILineParser lineParser = new LineParser(timeParser.Object);
            string errorMessage = string.Empty;
            try
            {
                lineParser.ParseLine("1y, invalid, invalid");
            }
            catch (InvalidLineException ex)
            {
                errorMessage = ex.Message;
            }

            //Assertions
            Assert.AreEqual(errorMessage.Contains("DataSet should be greater than 0"), true);
        }

        [TestMethod]
        public void ParseLine_InvalidLineException_Value_Invalid_data()
        {
            //Setup
            var timeParser = new Mock<ITimeParser>();

            //Operation
            ILineParser lineParser = new LineParser(timeParser.Object);
            string errorMessage = string.Empty;
            try
            {
                lineParser.ParseLine("1y, 20, invalid");
            }
            catch (InvalidLineException ex)
            {
                errorMessage = ex.Message;
            }

            //Assertions
            Assert.AreEqual(errorMessage.Contains("Value should be greater than 0"), true);
        }

        [TestMethod]
        public void ParseLine_InvalidLineException_Time_Invalid_data()
        {
            //Setup
            var timeParser = new Mock<ITimeParser>();

            //Operation
            ILineParser lineParser = new LineParser(timeParser.Object);

            var result = lineParser.ParseLine("4564, 20, 5");

            //Assertions
            Assert.AreEqual(result.TimeSpan, null);
        }

        [TestMethod]
        public void ParseLine_InvalidLineException_Valid_data()
        {
            //Setup
            var timeParser = new Mock<ITimeParser>();
            string time = "1y2m3w4d5m, 20, 15";
            timeParser.Setup(t => t.ParseTime(It.IsAny<string>())).Returns(new TimeModel() { Years = 1, Months = 2, Weeks = 3, Days = 4, Minutes = 5 });

            //Operation
            ILineParser lineParser = new LineParser(timeParser.Object);

            var result = lineParser.ParseLine(time);

            //Assertions
            Assert.AreNotEqual(result.TimeSpan, null);
            Assert.AreEqual(result.TimeSpan.Years, 1);
            Assert.AreEqual(result.TimeSpan.Months, 2);
            Assert.AreEqual(result.TimeSpan.Weeks, 3);
            Assert.AreEqual(result.TimeSpan.Days, 4);
            Assert.AreEqual(result.TimeSpan.Minutes, 5);
            Assert.AreEqual(result.DataSet, 20);
            Assert.AreEqual(result.Value, 15);
        }
    }
}
