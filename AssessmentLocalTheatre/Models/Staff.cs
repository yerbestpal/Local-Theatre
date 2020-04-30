using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssessmentLocalTheatre.Models
{
    public class Staff : ApplicationUser
    {
        // Properties.
        [Required]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        public string Role { get; set; }

        // Navigational properties.

        // Posts.
        public List<Post> Posts { get; set; }
    }
}