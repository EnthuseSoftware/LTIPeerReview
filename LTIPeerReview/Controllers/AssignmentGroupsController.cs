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
    public class AssignmentGroupsController : Controller
    {
        private PeerReviewModel db = new PeerReviewModel();

        // GET: AssignmentGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AssignmentGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssignmentAppID")] AssignmentGroup assignmentGroup)
        { 

            assignmentGroup.OrganizationID = Session["OrganizationID"].ToString();
            assignmentGroup.CourseID = (int)Session["CourseID"];
            assignmentGroup.ReviewAppID = Session["AssignmentName"].ToString();
            //check if assignmentgroup already exists
            var dbGroup = db.AssignmentGroups.Find(assignmentGroup.OrganizationID, assignmentGroup.CourseID, assignmentGroup.ReviewAppID);
            if (dbGroup == null && ModelState.IsValid)
            {
                //create new
                db.AssignmentGroups.Add(assignmentGroup);
                db.SaveChanges();
            }
            else
            {
                //edit old
                db.Entry(assignmentGroup).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("ReviewAdmin", "Tool");
        }

        // GET: AssignmentGroups/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignmentGroup assignmentGroup = db.AssignmentGroups.Find(id);
            if (assignmentGroup == null)
            {
                return HttpNotFound();
            }
            return View(assignmentGroup);
        }

        // POST: AssignmentGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrganizationID,CourseID,ReviewAppID,AssignmentAppID")] AssignmentGroup assignmentGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignmentGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(assignmentGroup);
        }

        // GET: AssignmentGroups/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignmentGroup assignmentGroup = db.AssignmentGroups.Find(id);
            if (assignmentGroup == null)
            {
                return HttpNotFound();
            }
            return View(assignmentGroup);
        }

        // POST: AssignmentGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AssignmentGroup assignmentGroup = db.AssignmentGroups.Find(id);
            db.AssignmentGroups.Remove(assignmentGroup);
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
