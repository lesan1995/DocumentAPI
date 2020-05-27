using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAPI.Models
{
    public class Document
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Cover { get; set; }
        public int PublishYear { get; set; }
    }
}
