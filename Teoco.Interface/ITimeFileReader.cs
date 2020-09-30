using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Teoco.Interface
{
    public interface ITimeFileReader
    {
        Task<List<LineModel>> ReadFile(); 
    }
}
