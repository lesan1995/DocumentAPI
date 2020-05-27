using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAPI.Models
{
    public class AddCategoryModel
    {
        [Required]
        public string Title { get; set; }
    }

    public class UpdateCategoryModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
