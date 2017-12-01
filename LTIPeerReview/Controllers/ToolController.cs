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

        public ActionResult ReviewAdmin()
        {
            return View();
        }

        // GET: Tool/LTISpoof
        [HttpGet]
        public ActionResult LTISpoof()
        {
            return View();
        }


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
            LTISpoof sessionData;
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
                Session["StudentID"] = ltiRequest.LisPersonEmailPrimary;
                //Session["StudentID"] = ltiRequest.LisPersonSourcedId;
                Session["OrganizationID"] = ltiRequest.ToolConsumerInstanceGuid;
                Session["CourseID"] = int.Parse(ltiRequest.ContextId);//TODO: replace with string; more versitile
                Session["UserRoles"] = ltiRequest.Roles;
                Session["AssignmentName"] = ltiRequest.ResourceLinkTitle;

                sessionData = new LTISpoof()
                {
                    Name = Session["StudentName"].ToString(),
                    StudentID = Session["StudentID"].ToString(),
                    OrganizationID = Session["OrganizationID"].ToString(),
                    CourseID = (int)Session["CourseID"],
                    Roles = Session["UserRoles"].ToString(),
                    AssignmentName = Session["AssignmentName"].ToString()
                };

                // The request is legit, so display the tool
                ViewBag.Message = string.Empty;
                var model = new ToolModel
                {
                    ConsumerSecret = "secret",
                    LtiRequest = ltiRequest
                };

                //check if user is an instructor or admin

                if (Session["UserRoles"] != null && (Session["UserRoles"].ToString().Contains("Instructor") || Session["UserRoles"].ToString().Contains("Admin")))
                {
                    //if they are, redirect to dashboard
                    return RedirectToAction("Admin");
                }

                using (PeerReviewModel db = new PeerReviewModel())
                {
                    //check if user exists
                    var student = db.Students.Find(sessionData.OrganizationID, sessionData.CourseID, sessionData.StudentID);
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

        public ActionResult Review()
        {
            Session["TestMessage"] = "Session Works!";
            LTISpoof sessionData;
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
                Session["StudentID"] = ltiRequest.LisPersonEmailPrimary;
                //Session["StudentID"] = ltiRequest.LisPersonSourcedId;
                Session["OrganizationID"] = ltiRequest.ToolConsumerInstanceGuid;
                Session["CourseID"] = int.Parse(ltiRequest.ContextId);//TODO: replace with string; more versitile
                Session["UserRoles"] = ltiRequest.Roles;
                Session["AssignmentName"] = ltiRequest.ResourceLinkTitle;

                sessionData = new LTISpoof()
                {
                    Name = Session["StudentName"].ToString(),
                    StudentID = Session["StudentID"].ToString(),
                    OrganizationID = Session["OrganizationID"].ToString(),
                    CourseID = (int)Session["CourseID"],
                    Roles = Session["UserRoles"].ToString(),
                    AssignmentName = Session["AssignmentName"].ToString()
                };

                // The request is legit, so display the tool
                ViewBag.Message = string.Empty;
                var model = new ToolModel
                {
                    ConsumerSecret = "secret",
                    LtiRequest = ltiRequest
                };

                

                //check if user is an instructor or admin

                if (sessionData.Roles != null && (sessionData.Roles.Contains("Instructor") || sessionData.Roles.Contains("Admin")))
                {
                    //if they are, redirect to dashboard
                    return RedirectToAction("ReviewAdmin");
                }


                using (PeerReviewModel db = new PeerReviewModel())
                {
                    //check if user exists
                    var student = db.Students.Find(sessionData.OrganizationID, sessionData.CourseID, sessionData.StudentID);

                   if (student == null)
                    {
                        //if user does not exist, display error
                        ViewBag.Message = "You have not been registered to use this tool.  Contact your instructor for assistance.";
                        return View();//TODO: redirect to error screen
                    }
                    else
                    {
                        //if they do, update the entry with their name
                        student.Name = sessionData.Name;
                        db.Entry(student).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    //create the view model
                    var viewModel = new ReviewHome()
                    {
                        ReviewerName = sessionData.Name,
                        AssignmentName = sessionData.AssignmentName,
                        FinishedReviews = student.Reviews.Count
                    };

                    //check that review tool has been registered
                    var assignmentGroup = db.AssignmentGroups.Find(sessionData.OrganizationID, sessionData.CourseID, sessionData.AssignmentName);
                    if (assignmentGroup == null || string.IsNullOrWhiteSpace(assignmentGroup.AssignmentAppID))
                    {
                        ViewBag.Message = "This review tool has not been registered with a submission assignment.  Contact your instructor to resolve this issue.";
                        return View(viewModel);//TODO: redirect to error screen
                    }
                    //get list of submissions in assignment group
                    List<Submission> submissions = db.Submissions
                        .Where(s => s.Submitter.OrganizationID == sessionData.OrganizationID
                            && s.Submitter.CourseID == sessionData.CourseID)
                        .ToList();
                    if (submissions == null || submissions.Count == 0)
                    {
                        ViewBag.Message = "No submissions found."
                            + "  Either no assignments have been submitted, or the review tool is registered incorrectly.";
                        return View(viewModel);
                    }

                    submissions = db.Submissions
                        .Where(s => s.Submitter.OrganizationID == sessionData.OrganizationID
                            && s.Submitter.CourseID == sessionData.CourseID
                            && s.Submitter.Group != student.Group)
                        .ToList();
                    if (submissions == null || submissions.Count == 0)
                    {
                        ViewBag.Message = "No submissions found for you to review."
                            + "  No student from another group has submitted an assignment yet.  Check back later.";
                        return View(viewModel);
                    }
                    return View(viewModel);
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