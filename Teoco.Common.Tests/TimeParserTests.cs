using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Teoco.Interface;

namespace Teoco.Common.Tests
{
    [TestClass]
    public class TimeParserTests
    {
        private TimeModel time;
        public TimeParserTests()
        {
            time = new TimeModel() { Years = 1, Months = 2, Weeks = 3, Days = 4, Minutes = 5 };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimeException), "ParseTime_InvalidTimeException")]
        public void ParseTime_InvalidTimeException()
        {

            //Setup
            //No setup

            //Operation
            GetProcessedDate(string.Empty);

            //Assertions
            //Exception assertion
        }

        [TestMethod]
        public void ParseTime_Years()
        {
            //Setup
            string time = "1y2m3w4d5m";

            //Operation
            TimeModel timeModel = GetProcessedDate(time);

            //Assertions
            Assert.AreEqual(timeModel.Years, 1);
        }

        [TestMethod]
        public void ParseTime_Months()
        {
            //Setup
            string time = "1y2m3w4d5m";

            //Operation
            TimeModel timeModel = GetProcessedDate(time);

            //Assertions
            Assert.AreEqual(timeModel.Months, 2);
        }

        [TestMethod]
        public void ParseTime_Weeks()
        {
            //Setup
            string time = "1y2m3w4d5m";

            //Operation
            TimeModel timeModel = GetProcessedDate(time);

            //Assertions
            Assert.AreEqual(timeModel.Weeks, 3);
        }

        [TestMethod]
        public void ParseTime_Days()
        {
            //Setup
            string time = "1y2m3w4d5m";

            //Operation
            TimeModel timeModel = GetProcessedDate(time);

            //Assertions
            Assert.AreEqual(timeModel.Days, 4);
        }

        [TestMethod]
        public void ParseTime_Minuites()
        {
            //Setup
            string time = "1y2m3w4d5m";

            //Operation
            TimeModel timeModel = GetProcessedDate(time);

            //Assertions
            Assert.AreEqual(timeModel.Minutes, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimeException), "ParseTime_InvalidTimeException")]
        public void ToTimeString_InvalidTimeException_NoData()
        {

            //Setup
            //No setup

            //Operation
            GetProcessedDate(new TimeModel());

            //Assertions
            //Exception assertion
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimeException), "ParseTime_InvalidTimeException")]
        public void ToTimeString_InvalidTimeException_Null()
        {

            //Setup
            TimeModel data = null;

            //Operation
            GetProcessedDate(data);

            //Assertions
            //Exception assertion
        }

        [TestMethod]
        public void ToTimeString_Years()
        {
            //Setup
            //Done in constructor

            //Operation
            string timeString = GetProcessedDate(time);

            //Assertions
            Assert.AreEqual(timeString.Contains("1y"), true);
        }

        [TestMethod]
        public void ToTimeString_Months()
        {
            //Setup
            //Done in constructor

            //Operation
            string timeString = GetProcessedDate(time);

            //Assertions
            Assert.AreEqual(timeString.Contains("2m"), true);
        }

        [TestMethod]
        public void ToTimeString_Weeks()
        {
            //Setup
            //Done in constructor

            //Operation
            string timeString = GetProcessedDate(time);

            //Assertions
            Assert.AreEqual(timeString.Contains("3w"), true);
        }

        [TestMethod]
        public void ToTimeString_Days()
        {
            //Setup
            //Done in constructor

            //Operation
            string timeString = GetProcessedDate(time);

            //Assertions
            Assert.AreEqual(timeString.Contains("4d"), true);
        }

        [TestMethod]
        public void ToTimeString_Minuites()
        {
            //Setup
            //Done in constructor

            //Operation
            string timeString = GetProcessedDate(time);

            //Assertions
            Assert.AreEqual(timeString.Contains("5m"), true);
        }

        private TimeModel GetProcessedDate(string timeString)
        {
            ITimeParser timeParser = new TimeParser();
            return timeParser.ParseTime(timeString);
        }

        private string GetProcessedDate(TimeModel time)
        {
            ITimeParser timeParser = new TimeParser();
            return timeParser.ToTimeString(time);
        }
    }
}
