using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LTIPeerReview.Models
{
    public class AssignmentGroup
    {
        [Key]
        [Column(Order = 1)]
        public string OrganizationID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int CourseID { get; set; }
        [Key]
        [Column(Order = 3)]
        public string ReviewAppID { get; set; }

        public string AssignmentAppID { get; set; }
    }
}