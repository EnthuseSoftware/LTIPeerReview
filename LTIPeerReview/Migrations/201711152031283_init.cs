namespace LTIPeerReview.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssignmentGroups",
                c => new
                    {
                        OrganizationID = c.String(nullable: false, maxLength: 128),
                        CourseID = c.Int(nullable: false),
                        ReviewAppID = c.String(nullable: false, maxLength: 128),
                        AssignmentAppID = c.String(),
                    })
                .PrimaryKey(t => new { t.OrganizationID, t.CourseID, t.ReviewAppID });
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FilePath = c.String(),
                        ReviewedSubmission_ID = c.Int(),
                        Reviewer_OrganizationID = c.String(maxLength: 128),
                        Reviewer_CourseID = c.Int(),
                        Reviewer_StudentID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Submissions", t => t.ReviewedSubmission_ID)
                .ForeignKey("dbo.Students", t => new { t.Reviewer_OrganizationID, t.Reviewer_CourseID, t.Reviewer_StudentID })
                .Index(t => t.ReviewedSubmission_ID)
                .Index(t => new { t.Reviewer_OrganizationID, t.Reviewer_CourseID, t.Reviewer_StudentID });
            
            CreateTable(
                "dbo.Submissions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AssignmentGroup = c.String(),
                        FilePath = c.String(),
                        Submitter_OrganizationID = c.String(maxLength: 128),
                        Submitter_CourseID = c.Int(),
                        Submitter_StudentID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Students", t => new { t.Submitter_OrganizationID, t.Submitter_CourseID, t.Submitter_StudentID })
                .Index(t => new { t.Submitter_OrganizationID, t.Submitter_CourseID, t.Submitter_StudentID });
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        OrganizationID = c.String(nullable: false, maxLength: 128),
                        CourseID = c.Int(nullable: false),
                        StudentID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Group = c.String(),
                    })
                .PrimaryKey(t => new { t.OrganizationID, t.CourseID, t.StudentID });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Submissions", new[] { "Submitter_OrganizationID", "Submitter_CourseID", "Submitter_StudentID" }, "dbo.Students");
            DropForeignKey("dbo.Reviews", new[] { "Reviewer_OrganizationID", "Reviewer_CourseID", "Reviewer_StudentID" }, "dbo.Students");
            DropForeignKey("dbo.Reviews", "ReviewedSubmission_ID", "dbo.Submissions");
            DropIndex("dbo.Submissions", new[] { "Submitter_OrganizationID", "Submitter_CourseID", "Submitter_StudentID" });
            DropIndex("dbo.Reviews", new[] { "Reviewer_OrganizationID", "Reviewer_CourseID", "Reviewer_StudentID" });
            DropIndex("dbo.Reviews", new[] { "ReviewedSubmission_ID" });
            DropTable("dbo.Students");
            DropTable("dbo.Submissions");
            DropTable("dbo.Reviews");
            DropTable("dbo.AssignmentGroups");
        }
    }
}
