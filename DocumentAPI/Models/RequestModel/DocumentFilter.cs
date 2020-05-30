using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAPI.Models
{
    public class DocumentFilter
    {
        public int? CategoryId { get; set; }
        public string Keyword { get; set; }
    }
}
