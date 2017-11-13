using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LTIPeerReview.Models
{
    public class Review
    {
        public int ID { get; set; }
        public Student Reviewer { get; set; }
        public Submission ReviewedSubmission { get; set; }
        public string FilePath { get; set; }
    }

}