using LTIPeerReview.DAL;
using LTIPeerReview.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LTIPeerReview
{
    public partial class ReviewUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LTISpoof sessionData;
            try
            {
                sessionData = new LTISpoof()
                {
                    Name = Session["StudentName"].ToString(),
                    StudentID = Session["StudentID"].ToString(),
                    OrganizationID = Session["OrganizationID"].ToString(),
                    CourseID = (int)Session["CourseID"],
                    Roles = Session["UserRoles"].ToString(),
                    AssignmentName = Session["AssignmentName"].ToString()
                };
            }
            catch(Exception ex)
            {
                StatusLabel.Text = "Session Error: " + ex.Message;
                return;
            }

            using (PeerReviewModel db = new PeerReviewModel())
            {
                //get student
                var student = db.Students.Where(s => s.OrganizationID == sessionData.OrganizationID
                            && s.CourseID == sessionData.CourseID
                            && s.StudentID == sessionData.StudentID).Include("Reviews")
                            .Include("Reviews.ReviewedSubmission")
                            .Include("Reviews.ReviewedSubmission.Submitter")
                            .FirstOrDefault();
                //var student = db.Students.Find(sessionData.OrganizationID, sessionData.CourseID, sessionData.StudentID);
                //db.Entry(student).Reference(s => s.Reviews).Load();
                if (student == null)
                {
                    //if user does not exist, display error
                    StatusLabel.Text = "You have not been registered to use this tool.  Contact your instructor for assistance.";
                    return;//TODO: redirect to error screen
                }
                //check if student has unfinished review
                var reviewList = student.Reviews.ToList();
                for (int j = 0; j < reviewList.Count; j++)
                {
                    var r = reviewList[j];
                    db.Entry(r).Reference(s => s.ReviewedSubmission).Load();
                    if (String.IsNullOrWhiteSpace(r.FilePath))
                    {
                        //display incomplete review
                        SubmitterNameLabel.Text = r.ReviewedSubmission.Submitter.Name;
                        SubmissionNameLabel.Text = r.ReviewedSubmission.AssignmentGroup;
                        GroupLabel.Text = r.ReviewedSubmission.Submitter.Group;
                        //save review id to session
                        Session["ReviewID"] = r.ID;
                        return;
                    }
                }
                foreach (Review r in student.Reviews)
                {
                    
                }
                
                List<Submission> submissions = new List<Submission>();
                int i = 0;
                do
                {
                    submissions = db.Submissions
                    .Where(s => s.Submitter.OrganizationID == sessionData.OrganizationID
                        && s.Submitter.CourseID == sessionData.CourseID
                        && s.Submitter.Group != student.Group
                        && s.Reviews.Count <= i)
                        .Include("Submitter")
                    .ToList();
                    i++;
                } while (submissions == null || submissions.Count < 1 || i > 2);
                if (submissions == null || submissions.Count == 0)
                {
                    StatusLabel.Text = "All submissions reviewed."
                        + "  Each submitted assignment from another group already has 3 or more reviews.  Check back later.";
                }
                else
                {
                    //get random submission
                    Random rand = new Random();
                    int randNum = rand.Next(submissions.Count);
                    var submission = submissions[randNum];
                    if (submission == null)
                    {
                        StatusLabel.Text = "Error: the randomly selected submission was null!";
                        return;
                    }
                    //add incomplete review
                    Review review = new Review()
                    {
                        Reviewer = student,
                        ReviewedSubmission = submission
                    };
                    db.Reviews.Add(review);
                    db.SaveChanges();
                    //display incomplete review
                    SubmitterNameLabel.Text = review.ReviewedSubmission.Submitter.Name;
                    SubmissionNameLabel.Text = review.ReviewedSubmission.AssignmentGroup;
                    GroupLabel.Text = review.ReviewedSubmission.Submitter.Group;
                    //save review id to session
                    Session["ReviewID"] = review.ID;
                }
                //get list of submissions with i++ reviews belonging to a different student group

                //display review input form for random submission
            }

        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (FileUploadControl.HasFile)
            {
                LTISpoof sessionData;
                int reviewID;
                string filename, filepath;
                //string org = Session["OrganizationID"].ToString();
                //int course = (int)Session["CourseID"];
                //string lab = Session["AssignmentName"].ToString();
                //string lnum = Session["StudentID"].ToString();

                //string filepath = Session["OrganizationID"].ToString() + "/"
                //        + Session["CourseID"].ToString() + "/"
                //        + Session["AssignmentName"].ToString() + "/"
                //        + Session["StudentID"].ToString() + "/";
                try
                {
                    sessionData = new LTISpoof()
                    {
                        Name = Session["StudentName"].ToString(),
                        StudentID = Session["StudentID"].ToString(),
                        OrganizationID = Session["OrganizationID"].ToString(),
                        CourseID = (int)Session["CourseID"],
                        Roles = Session["UserRoles"].ToString(),
                        AssignmentName = Session["AssignmentName"].ToString()
                    };
                    reviewID = (int)Session["ReviewID"];

                    filename = Path.GetFileName(FileUploadControl.FileName);
                    filepath = Server.MapPath("~/App_Data/" + filename);
                    FileUploadControl.SaveAs(filepath);
                    StatusLabel.Text = "Upload status: File uploaded! ";

                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message + " : " + ex.StackTrace;
                    return;
                }

                
                using (PeerReviewModel db = new PeerReviewModel())
                {
                    //retrieve incomplete review
                    var review = db.Reviews.Find((int)Session["ReviewID"]);
                    if (review == null)
                    {
                        StatusLabel.Text = "Error: Review could not be retrieved.";
                        return;
                    }

                    review.FilePath = filepath;
                    db.Entry(review).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
        }

        protected void DownloadButton_Click(object sender, EventArgs e)
        {
            using (PeerReviewModel db = new PeerReviewModel())
            {
                //retrieve incomplete review
                //var review = db.Reviews.Find((int)Session["ReviewID"]);
                int id = (int)Session["ReviewID"];
                var review = db.Reviews.Where(r => r.ID == id)
                            .Include("ReviewedSubmission")
                            .FirstOrDefault();
                if (review == null)
                {
                    StatusLabel.Text = "Error: Review could not be retrieved.";
                }
                else if (review.ReviewedSubmission == null)
                {
                    StatusLabel.Text = "Error: Submission could not be retrieved.";
                }
                else
                {
                    //WebClient client = new WebClient();
                    string filepath = review.ReviewedSubmission.FilePath;
                    string filename = Path.GetFileName(filepath);
                    //client.DownloadFile(filepath, filename);
                    //Response.Redirect(filepath);
                    
                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
                    string aaa = Server.MapPath("~/App_Data/" + filename);
                    Response.TransmitFile(Server.MapPath("~/App_Data/" + filename));
                    Response.End();
                    StatusLabel.Text = "Submission Files downloaded.";
                }

            }
        }
    }
}