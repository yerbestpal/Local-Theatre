using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AssessmentLocalTheatre.Models
{
    /// <summary>
    /// POCO class for comments.
    /// Comments are used to reply to Posts and other comments.
    /// </summary>
    public class Comment
    {
        //Default constructor
        public Comment()
        {

        }

        // Properties.
        [Key]
        public int CommentId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Content { get; set; }

        // Navigational properties.

        /// <summary>
        /// One Post has many Comments.
        /// </summary>
        [ForeignKey("Post")]
        public int PostId { get; set; }

        [Required]
        public Post Post { get; set; }

        /// <summary>
        /// One Comment has one ApplicationUser. 
        /// ApplicationUser is the author of the Comment.
        /// </summary>
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}   