using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AssessmentLocalTheatre.Models
{
    public class Post
    {
        /// <summary>
        /// Default constructor. This applies property values that can be determined at instantiation time, independent of other factors.
        /// </summary>
        public Post()
        {
            this.IsApproved = true;  // Posts are approved by default.
            this.PostDate = DateTime.Now;
            this.Comments = new List<Comment>();
            this.Description = "";
        }

        // Properties.

        [Key]
        public int PostId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Content { get; set; }

        // These properties do not need 'Required' annotations since they will be handled automatically by the system.
        public DateTime PostDate { get; set; }

        // By default, IsApproved will be false.
        public bool IsApproved { get; set; }


        // Navigation Properties.

        // Category
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Staff.
        [ForeignKey("Staff")]
        public string StaffId { get; set; }
        public Staff Staff { get; set; }

        // Comments.
        public List<Comment> Comments { get; set; }
    }
}