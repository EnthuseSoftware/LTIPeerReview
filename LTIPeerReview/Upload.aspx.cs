using LTIPeerReview.DAL;
using LTIPeerReview.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LTIPeerReview
{
    public partial class Upload : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (FileUploadControl.HasFile)
            {
                string filename = "File upload failed";
                string org = Session["OrganizationID"].ToString();
                int course = (int)Session["CourseID"];
                string lab = Session["AssignmentName"].ToString();
                string lnum = Session["StudentID"].ToString();

                string filepath = Session["OrganizationID"].ToString() + "/"
                        + Session["CourseID"].ToString() + "/"
                        + Session["AssignmentName"].ToString() + "/"
                        + Session["StudentID"].ToString() + "/";
                try
                {
                    
                    filename = Path.GetFileName(FileUploadControl.FileName);
                    FileUploadControl.SaveAs(Server.MapPath("~/App_Data/" + filename));
                    StatusLabel.Text = "Upload status: File uploaded! ";

                    
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message + "/n" + ex.StackTrace;
                }

                //add submission to database
                using (PeerReviewModel db = new PeerReviewModel())
                {
                    //db.Database.Delete();
                    //db.Database.Create();
                    //get student
                    var student = db.Students.Find(org, course, lnum);
                    if (student == null)
                    {
                        //throw error
                    }

                    //get list of student's submissions
                    var studentSubmissions = student.Submissions;

                    if (studentSubmissions != null && studentSubmissions.Count > 0)
                    {
                        //update submission with new file
                        foreach (Submission s in studentSubmissions)
                        {
                            if (s.AssignmentGroup == Session["AssignmentName"].ToString() && s.Submitter.Equals(student))
                            {
                                //TODO: delete old file
                                s.FilePath = Server.MapPath("~/App_Data/" + filename);
                                db.Entry(s).State = EntityState.Modified;
                                db.SaveChanges();
                                return;
                            }
                        }
                    }
                    
                    //create submission
                    db.Submissions.Add(new Submission()
                    {
                        Submitter = student,
                        AssignmentGroup = Session["AssignmentName"].ToString(),
                        FilePath = filepath + filename
                    });
                    db.SaveChanges();


                }

            }
        }
    }
}