using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LTIPeerReview.DAL;
using LTIPeerReview.Models;
using System.IO;
using System.Web.UI.HtmlControls;

namespace LTIPeerReview.Controllers
{
    public class ReviewsController : Controller
    {
        private PeerReviewModel db = new PeerReviewModel();

        // GET: Reviews
        public ActionResult Index()
        {
            return View(db.Reviews.ToList());
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        public ActionResult Create()
        {
            //var fileName = "";
            //if (Request.RequestType == "POST")
            //{
            //    var fileSavePath = "";
                
            //    try
            //    {
            //        //var uploadedFile = Request.Files[0];
            //        var uploadFile = (HtmlInputFile)(Request.Form["reviewUpload"]);
            //        fileName = Path.GetFileName(uploadedFile.FileName);
            //        fileSavePath = Server.MapPath("~/App_Data/" + fileName);
            //        uploadedFile.SaveAs(fileSavePath);

            //        ViewBag.Message = "Upload status: File uploaded! " + fileSavePath;
            //        return RedirectToAction("Review", "Tool");
            //    }
            //    catch (Exception ex)
            //    {
            //        ViewBag.Message = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message + " : " + ex.StackTrace;
            //        return View();
            //    }
            //}
            //GET: set review data
            //check for unfinished review

            //create new review for random submission

            return View();
            
            ////add submission to database
            //using (PeerReviewModel db = new PeerReviewModel())
            //{
            //    //db.Database.Delete();
            //    //db.Database.Create();
            //    //get student
            //    var student = db.Students.Find(org, course, lnum);
            //    if (student == null)
            //    {
            //        //throw error
            //    }

            //    //get list of student's submissions
            //    var studentSubmissions = student.Submissions;

            //    if (studentSubmissions != null && studentSubmissions.Count > 0)
            //    {
            //        //update submission with new file
            //        foreach (Submission s in studentSubmissions)
            //        {
            //            if (s.AssignmentGroup == Session["AssignmentName"].ToString() && s.Submitter.Equals(student))
            //            {
            //                //TODO: delete old file
            //                s.FilePath = Server.MapPath("~/App_Data/" + filename);
            //                db.Entry(s).State = EntityState.Modified;
            //                db.SaveChanges();
            //                return;
            //            }
            //        }
            //    }

            //    //create submission
            //    db.Submissions.Add(new Submission()
            //    {
            //        Submitter = student,
            //        AssignmentGroup = Session["AssignmentName"].ToString(),
            //        FilePath = filepath + filename
            //    });
            //    db.SaveChanges();


            //}
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FilePath")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
