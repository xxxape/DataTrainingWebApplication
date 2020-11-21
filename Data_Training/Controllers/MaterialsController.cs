using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Data_Training.Models;
using Data_Training.Utils;
using Microsoft.AspNet.Identity;

namespace Data_Training.Controllers
{
    public class MaterialsController : Controller
    {
        private DataTrainingModels db = new DataTrainingModels();

        // GET: Materials
        public ActionResult Index()
        {
            return View(db.Material.ToList());
        }

        // GET: Materials/Details/5
        [Authorize(Roles = "tutor,admin,student")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Material.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            return View(material);
        }

        // GET: Materials/Create
        [Authorize(Roles = "tutor,admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Materials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "tutor,admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,UploadDate")] Material material, HttpPostedFileBase postedFile)
        {
            material.TutorId = User.Identity.GetUserId();
            var myUniqueFileName = string.Format(@"{0}", Guid.NewGuid());
            material.Path = postedFile.FileName.Split('.')[0] + "_" + myUniqueFileName;
            ModelState.Clear();
            TryValidateModel(material);
            if (ModelState.IsValid)
            {
                string serverPath = Server.MapPath("~/Uploads/");
                string fileExtension = Path.GetExtension(postedFile.FileName);
                string filePath = material.Path + fileExtension;
                material.Path = filePath;
                postedFile.SaveAs(serverPath + material.Path);
                db.Material.Add(material);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(material);
        }

        // GET: Materials/Edit/5
        [Authorize(Roles = "tutor,admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Material.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            return View(material);
        }

        // POST: Materials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "tutor,admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,UploadDate,Path")] Material material, HttpPostedFileBase postedFile)
        {
            material.TutorId = User.Identity.GetUserId();
           
            ModelState.Clear();
            TryValidateModel(material);
            if (ModelState.IsValid)
            {
                if(postedFile != null)
                {
                    string serverPath = Server.MapPath("~/Uploads/");
                    // delete existing file
                    System.IO.File.Delete(serverPath + material.Path);
                    var myUniqueFileName = string.Format(@"{0}", Guid.NewGuid());
                    material.Path = postedFile.FileName.Split('.')[0] + "_" + myUniqueFileName;
                    string fileExtension = Path.GetExtension(postedFile.FileName);
                    string filePath = material.Path + fileExtension;
                    material.Path = filePath;
                    postedFile.SaveAs(serverPath + material.Path);
                }
                db.Entry(material).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(material);
        }

        // GET: Materials/Delete/5
        [Authorize(Roles = "tutor,admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Material.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            return View(material);
        }

        // POST: Materials/Delete/5
        [Authorize(Roles = "tutor,admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Material material = db.Material.Find(id);

            // delete file
            string severPath = Server.MapPath("~/Uploads/");
            System.IO.File.Delete(severPath + material.Path);

            db.Material.Remove(material);
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

        // GET: Materials/Download/5
        [Authorize(Roles = "tutor,admin,student")]
        public ActionResult Download(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Material.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            var emailAddress = User.Identity.GetUserName();
            EmailSender emailSender = new EmailSender();
            string filePath = Server.MapPath("~/Uploads/") + material.Path;
            emailSender.Send(emailAddress, "Learning Material", "The attachemnt file is the learning material that you have downloaded.", filePath, material.Path);

            return RedirectToAction("Index");
        }
    }
}
