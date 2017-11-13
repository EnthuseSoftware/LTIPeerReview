using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LTIPeerReview.Models
{
    public class Group
    {
        [Key]
        public int ID { get; set; }
        public string OrganizationID { get; set; }
        public int CourseID { get; set; }
        public string GroupName { get; set; }
        public virtual ICollection<Student> Students { get; set; }

    }
}