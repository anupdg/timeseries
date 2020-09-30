using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teoco.Interface
{
    public interface ITimeFileWriter
    {
        Task WriteToFile(string filePath, IOrderedEnumerable<LineModel> orderedQuery);
    }
}
