using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LTIPeerReview.Models
{
    public class LTISpoof
    {
        public string OrganizationID { get; set; }
        public int CourseID { get; set; }
        public string StudentID { get; set; }
        public string Name { get; set; }
        public string Roles { get; set; }
        public string AssignmentName { get; set; }

    }
}