using System;
using System.Collections.Generic;
using System.Text;

namespace Teoco.Interface
{
    /// <summary>
    /// Line model
    /// </summary>
    public class LineModel
    {
        public TimeModel TimeSpan { get; set; }
        public int DataSet { get; set; }
        public int Value { get; set; }
    }
}
