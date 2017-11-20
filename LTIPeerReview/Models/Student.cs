using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LTIPeerReview.Models
{
    public class Student
    {
        [Key]
        [Column(Order = 1)]
        public string OrganizationID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int CourseID { get; set; }
        [Key]
        [Column(Order = 3)]
        public string StudentID { get; set; }
        
        public string Name { get; set; }

        public string Group { get; set; }

        public virtual ICollection<Submission> Submissions { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}