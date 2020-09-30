using System;
using System.Collections.Generic;
using System.Text;

namespace Teoco.Interface
{
    public interface ILineParser
    {
        LineModel ParseLine(string line);
    }
}
