using System;
using System.Collections.Generic;
using System.Text;

namespace Teoco.Interface
{
    public interface ITimeParser
    {
        TimeModel ParseTime(string timeStr);

        string ToTimeString(TimeModel timeModel);
    }
}
