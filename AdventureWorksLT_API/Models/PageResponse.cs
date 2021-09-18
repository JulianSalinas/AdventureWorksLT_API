using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorksLT_API.Models
{
    public class PageResponse<T>
    {
        public long TotalRecords { get; set; }
        public List<T> PageRecords { get; set; }
    }
}
