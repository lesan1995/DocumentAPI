using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAPI.Models
{
    public class AddDocumentModel
    {
        public int? CategoryId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string Cover { get; set; }
        [Required]
        public int PublishYear { get; set; }
    }

    public class UpdateDocumentModel
    {
        [Required]
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string Cover { get; set; }
        [Required]
        public int PublishYear { get; set; }
    }
}
