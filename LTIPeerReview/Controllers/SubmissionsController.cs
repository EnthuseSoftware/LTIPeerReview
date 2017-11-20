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

namespace LTIPeerReview.Controllers
{
    public class SubmissionsController : Controller
    {
        private PeerReviewModel db = new PeerReviewModel();

        // GET: Submissions
        public ActionResult Index()
        {
            string org = Session["OrganizationID"].ToString();
            int course = (int)Session["CourseID"];

            var submissions = db.Submissions
                    .Where(s => s.Submitter.OrganizationID == org && s.Submitter.CourseID == course)
                    .ToList();
            return View(submissions);
        }

        // GET: Submissions/Details/5
        public ActionResult Details()
        {
            //string org = Session["OrganizationID"].ToString();
            //int course = (int)Session["CourseID"];
            //string user = Session["StudentID"].ToString();
            //string assignment = Session["AssignmentName"].ToString();

            //var student = db.Students.Find(org, course, user);
            //if (student != null)
            //{

            //}
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Submission submission = db.Submissions.Find(id);
            //if (submission == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(submission);
            return View();
        }

        // GET: Submissions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Submissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AssignmentGroup,FilePath")] Submission submission)
        {
            if (ModelState.IsValid)
            {
                db.Submissions.Add(submission);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(submission);
        }

        // GET: Submissions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submission submission = db.Submissions.Find(id);
            if (submission == null)
            {
                return HttpNotFound();
            }
            return View(submission);
        }

        // POST: Submissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AssignmentGroup,FilePath")] Submission submission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(submission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(submission);
        }

        // GET: Submissions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submission submission = db.Submissions.Find(id);
            if (submission == null)
            {
                return HttpNotFound();
            }
            return View(submission);
        }

        // POST: Submissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Submission submission = db.Submissions.Find(id);
            db.Submissions.Remove(submission);
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
