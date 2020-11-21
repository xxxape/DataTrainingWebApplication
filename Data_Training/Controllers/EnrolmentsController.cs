using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Data_Training.Models;
using Microsoft.AspNet.Identity;

namespace Data_Training.Controllers
{
    public class EnrolmentsController : Controller
    {
        private DataTrainingModels db = new DataTrainingModels();

        // GET: Enrolments
        [Authorize(Roles = "student")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var enrolment = db.Enrolment.Where(e => e.StuId == userId);
            
            return View(enrolment.ToList());
        }

        // GET: Enrolments/Create
        [Authorize(Roles = "student")]
        public ActionResult Create()
        {
            ViewBag.Cid = new SelectList(db.Lesson, "Id", "Name");
            return View();
        }

        // POST: Enrolments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Cid")] Enrolment enrolment)
        {
            enrolment.StuId = User.Identity.GetUserId();

            ModelState.Clear();
            TryValidateModel(enrolment);
            if (ModelState.IsValid)
            {
                db.Enrolment.Add(enrolment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Cid = new SelectList(db.Lesson, "Id", "Name", enrolment.Cid);
            return View(enrolment);
        }

        // GET: Enrolments/Edit/5
        [Authorize(Roles = "student")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrolment enrolment = db.Enrolment.Find(id);
            if (enrolment == null)
            {
                return HttpNotFound();
            }
            var items = new List<SelectListItem>()
            {
                (new SelectListItem(){Text = "1", Value = "1"}),
                (new SelectListItem(){Text = "2", Value = "2"}),
                (new SelectListItem(){Text = "3", Value = "3"}),
                (new SelectListItem(){Text = "4", Value = "4"}),
                (new SelectListItem(){Text = "5", Value = "5"}),
                (new SelectListItem(){Text = "6", Value = "6"}),
                (new SelectListItem(){Text = "7", Value = "7"}),
                (new SelectListItem(){Text = "8", Value = "8"}),
                (new SelectListItem(){Text = "9", Value = "9"}),
                (new SelectListItem(){Text = "10", Value = "10"})
            };
            ViewBag.ratingList = items;
            ViewBag.rating = enrolment.Rating;
            ViewBag.Cid = new SelectList(db.Lesson, "Id", "Name", enrolment.Cid);
            return View(enrolment);
        }

        // POST: Enrolments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cid,Rating")] Enrolment enrolment)
        {
            enrolment.StuId = User.Identity.GetUserId();

            ModelState.Clear();
            TryValidateModel(enrolment);
            if (ModelState.IsValid)
            {
                db.Entry(enrolment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Cid = new SelectList(db.Lesson, "Id", "Name", enrolment.Cid);
            return View(enrolment);
        }

        // GET: Enrolments/Delete/5
        [Authorize(Roles = "student")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrolment enrolment = db.Enrolment.Find(id);
            if (enrolment == null)
            {
                return HttpNotFound();
            }
            return View(enrolment);
        }

        // POST: Enrolments/Delete/5
        [Authorize(Roles = "student")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrolment enrolment = db.Enrolment.Find(id);
            db.Enrolment.Remove(enrolment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST:Enrolments/Rating/
        [Authorize(Roles = "student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rating(int? id)
        {
            //var rating = 
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
