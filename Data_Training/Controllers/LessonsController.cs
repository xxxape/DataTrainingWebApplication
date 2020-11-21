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
using Newtonsoft.Json;

namespace Data_Training.Controllers
{
    public class LessonsController : Controller
    {
        private DataTrainingModels db = new DataTrainingModels();

        // GET: Lessons
        public ActionResult Index()
        {
            var lesson = db.Lesson.Include(l => l.Classroom).ToList(); 
            string[] names = new string[lesson.Count];
            int[] nums = new int[lesson.Count];
            for (int i = 0; i < lesson.Count; i++)
            {
                names[i] = lesson[i].Name;
                var lesson_id = lesson[i].Id;
                var ratings = db.Enrolment.Where(l => l.Cid == lesson_id).ToList();
                var sum = 0;
                decimal avg = 0;
                var count = 0;
                for(int j = 0; j < ratings.Count; j++)
                {
                    var rating = ratings[j].Rating;
                    if (rating != 0)
                    {
                        sum += rating;
                        count++;
                    }
                }
                if(count != 0)
                {
                    avg = (decimal)sum / count;
                }                
                lesson[i].AvgOfRatings = avg;
                lesson[i].NumOfEnrolment = ratings.Count;
                nums[i] = ratings.Count;
                ModelState.Clear();
                TryValidateModel(lesson[i]);
                if (ModelState.IsValid)
                {
                    db.Entry(lesson[i]).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            lesson = db.Lesson.Include(l => l.Classroom).ToList();
            string jsonNames = JsonConvert.SerializeObject(names);
            string jsonNums = JsonConvert.SerializeObject(nums);
            ViewBag.names = jsonNames;
            ViewBag.nums = jsonNums;
            
            return View(lesson);
        }

        // GET: Lessons/Create
        [Authorize(Roles = "tutor,admin")]
        public ActionResult Create()
        {
            ViewBag.Crid = new SelectList(db.Classroom, "Id", "Address");
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "tutor,admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DateTime,Crid")] Lesson lesson)
        {
            lesson.TutorId = User.Identity.GetUserId();

            ModelState.Clear();
            TryValidateModel(lesson);
            if (ModelState.IsValid)
            {
                db.Lesson.Add(lesson);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Crid = new SelectList(db.Classroom, "Id", "Address", lesson.Crid);
            return View(lesson);
        }

        // GET: Lessons/Edit/5
        [Authorize(Roles = "tutor,admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lesson.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            ViewBag.Crid = new SelectList(db.Classroom, "Id", "Address", lesson.Crid);
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "tutor,admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DateTime,Crid")] Lesson lesson)
        {
            lesson.TutorId = User.Identity.GetUserId();

            ModelState.Clear();
            TryValidateModel(lesson);
            if (ModelState.IsValid)
            {
                db.Entry(lesson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Crid = new SelectList(db.Classroom, "Id", "Address", lesson.Crid);
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lesson.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lesson lesson = db.Lesson.Find(id);
            db.Lesson.Remove(lesson);
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
