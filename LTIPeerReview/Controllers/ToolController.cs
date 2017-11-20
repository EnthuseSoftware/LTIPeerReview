using LtiLibrary.AspNet.Extensions;
using LtiLibrary.Core.Common;
using LtiLibrary.Core.Lti1;
using LTIPeerReview.DAL;
using LTIPeerReview.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTIPeerReview.Controllers
{
    public class ToolController : Controller
    {
        // GET: Tools
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }

        // GET: Tool/LTISpoof
        public ActionResult LTISpoof()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LTISpoof([Bind(Include = "OrganizationID,CourseID,StudentID,Name,Roles,AssignmentName")] LTISpoof spoof)
        {
            Session.Add("StudentName", spoof.Name);
            Session["StudentName"] = spoof.Name;
            Session["StudentID"] = spoof.StudentID;
            Session["OrganizationID"] = spoof.OrganizationID;
            Session["CourseID"] = spoof.CourseID;
            Session["UserRoles"] = spoof.Roles;
            Session["AssignmentName"] = spoof.AssignmentName;
            ViewBag.Message = spoof.Name + " / " + Session["StudentName"].ToString();
            return View(spoof);
        }

        public ActionResult Upload()
        {
            Session["TestMessage"] = "Session Works!";
            try
            {
                // Parse and validate the request
                Request.CheckForRequiredLtiParameters();

                var ltiRequest = new LtiRequest(null);
                ltiRequest.ParseRequest(Request);

                if (!ltiRequest.ConsumerKey.Equals("12345"))
                {
                    ViewBag.Message = "Invalid Consumer Key";
                    return View();
                }

                var oauthSignature = Request.GenerateOAuthSignature("secret");
                if (!oauthSignature.Equals(ltiRequest.Signature))
                {
                    ViewBag.Message = "Invalid Signature";
                    return View();
                }

                Session["StudentName"] = ltiRequest.LisPersonNameFull;
                Session["StudentID"] = ltiRequest.LisPersonSourcedId;
                Session["OrganizationID"] = ltiRequest.ContextOrg;
                Session["CourseID"] = ltiRequest.ContextId;
                Session["UserRoles"] = ltiRequest.Roles;
                Session["AssignmentName"] = ltiRequest.ResourceLinkTitle;

                // The request is legit, so display the tool
                ViewBag.Message = string.Empty;
                var model = new ToolModel
                {
                    ConsumerSecret = "secret",
                    LtiRequest = ltiRequest
                };

                //check if user is an instructor or admin
                
                if (Session["Roles"].ToString().Contains("instructor") || Session["Roles"].ToString().Contains("admin"))
                {
                    //if they are, redirect to dashboard
                }

                using (PeerReviewModel db = new PeerReviewModel())
                {
                    //check if user exists
                    var student = db.Students.Find(ltiRequest.LisPersonSourcedId, ltiRequest.ContextOrg);
                    if (student == null)
                    {
                        //if user does not exist, display error
                        ViewBag.Message = "You have not been registered to use this tool.  Contact your instructor for assistance.";
                        return View();
                    }
                    else
                    {
                        //if they do, update the entry with their name
                        student.Name = Session["StudentName"].ToString();
                        db.Entry(student).State = EntityState.Modified;
                        db.SaveChanges();
                        return Redirect(url: "~/upload.aspx");
                    }
                }

            }
            catch (LtiException e)
            {
                ViewBag.Message = e.Message;
                //return Redirect(url: "~/upload.aspx");
                return View();
            }
        }
    }
}