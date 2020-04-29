using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssessmentLocalTheatre.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }


        // Navigation Properties.

        // Posts.
        public List<Post> Posts { get; set; }
    }
}