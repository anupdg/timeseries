using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teoco.Interface
{
    public interface IProcessData
    {
        IOrderedEnumerable<LineModel> SortByTime(List<LineModel> timeData);
        IOrderedEnumerable<LineModel> SortByDataSet(List<LineModel> timeData);
        Task<Tuple<string, string>> Process(DateTime processTime);
    }
}
