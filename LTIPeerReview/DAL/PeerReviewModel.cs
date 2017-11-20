using LTIPeerReview.Models;
using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace LTIPeerReview.DAL
{

    public class PeerReviewModel : DbContext
    {
        public PeerReviewModel() : base("name=PeerReviewModel")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<AssignmentGroup> AssignmentGroups { get; set; }

    }
}