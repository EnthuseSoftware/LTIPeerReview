using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LTIPeerReview.Models
{
    public class Submission
    {
        [Key]
        public int ID { get; set; }
        public Student Submitter { get; set; }
        public string AssignmentGroup { get; set; }
        public string FilePath { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}