using LTIPeerReview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LTIPeerReview.DAL
{
    public class DataInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<PeerReviewModel>
    {
        protected override void Seed(PeerReviewModel context)
        {
            //var students = new List<Student>
            //{

            //};

            //students.ForEach(s => context.Students.Add(s));
            //context.SaveChanges();
            base.Seed(context);
        }
    }
}