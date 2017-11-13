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
                try
                {
                    filename = Path.GetFileName(FileUploadControl.FileName);
                    FileUploadControl.SaveAs(Server.MapPath("~/") + filename);
                    StatusLabel.Text = "Upload status: File uploaded! " + (string)Session["TestMessage"];

                    
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }

                //add submission to database
                using (PeerReviewModel db = new PeerReviewModel())
                {
                    //db.Database.Delete();
                    //db.Database.Create();
                    db.Submissions.Add(new Submission()
                    {
                        //Submitter = new Student() { ID = "Cooper.T.0043@Gmail.com", Name = "Timothy Cooper" },
                        AssignmentGroup = "TestLab",
                        FilePath = Server.MapPath("~/") + filename,
                        Reviews = new List<Review>()
                    });
                    db.SaveChanges();


                }

            }
        }
    }
}