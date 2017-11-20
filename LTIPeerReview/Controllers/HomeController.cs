using LtiLibrary.AspNet.Extensions;
using LtiLibrary.Core.Common;
using LtiLibrary.Core.Lti1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using LTIPeerReview.Models;
using LTIPeerReview.DAL;

namespace LTIPeerReview.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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

                Session["UploaderName"] = ltiRequest.LisPersonNameFull;
                Session["UploaderID"] = ltiRequest.LisPersonSourcedId;
                Session["OrganizationID"] = ltiRequest.ContextOrg;
                Session["CourseID"] = ltiRequest.ContextId;
                Session["Roles"] = ltiRequest.Roles;
                //Session["UploaderGroupId"] = ltiRequest.LisGroupSourcedId;
                //Session["AssignmentId"] = ltiRequest.ResourceLinkId;
                // The request is legit, so display the tool
                ViewBag.Message = string.Empty;
                var model = new ToolModel
                {
                    ConsumerSecret = "secret",
                    LtiRequest = ltiRequest
                };

                //check if user exists
                //if user does not exist, create one and add to agroup

                using (PeerReviewModel db = new PeerReviewModel())
                {
                    var student = db.Students.Find(ltiRequest.LisPersonSourcedId);
                    if (student == null)
                    {
                        db.Students.Add(new Student() {
                            OrganizationID = ltiRequest.ContextOrg,
                            StudentID = ltiRequest.LisPersonSourcedId,
                            Name = ltiRequest.LisPersonNameFull
                        });
                        db.SaveChanges();
                    }
                }
                return Redirect(url: "~/upload.aspx");

            }
            catch (LtiException e)
            {
                ViewBag.Message = e.Message;
                return Redirect(url: "~/upload.aspx");
                //return View();
            }
        }

    }
}