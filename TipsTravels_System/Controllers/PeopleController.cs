using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TipsTravels_System.Models;
using TipsTravels_System.ViewModel;

namespace TipsTravels_System.Controllers
{
    public class PeopleController : Controller
    {
        private TripsDBEntities db = new TripsDBEntities();
        // GET: People
        public ActionResult Index()
        {
            var persons = db.Persons.Include(p => p.Role1);
            return View(persons.ToList().Where(_ => _.Role is 2 or 3));
        }
        
   
        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }
     

        // GET: People/Create
        public ActionResult Create()
        {
            ViewBag.Role = new SelectList(db.Roles, "ID", "RoleName");
            return View(new PersonViewModel());
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 

        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonViewModel person)
        {

            if (ModelState.IsValid)
            {
                if (Request.Form.Get("Chbtn") == "Upload")
                {
                    ViewBag.Role = new SelectList(db.Roles, "ID", "RoleName");
                    PublicObjects.Image = PublicObjects.ConvertToBytes(person.file);
                    return View(new PersonViewModel() { Photo = PublicObjects.Image });
                }
                person.Photo = PublicObjects.Image;
                db.InsertUsers(person.FName, person.LName, person.Mail, person.Phone, person.Photo, person.Role,
                           person.Username, person.PasswordAsString);
                return RedirectToAction("Index");
            }
            ViewBag.Role = new SelectList(db.Roles, "ID", "RoleName", person.Role);
            return View(person);
        }

        // GET: People/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person _ = db.Persons.Find(id);
            //    PersonViewModel person = new PersonViewModel() { PersonID = _.PersonID, FName = _.FName, LName = _.LName, Mail = _.Mail, Phone = _.Phone, Photo = _.Photo, Password = _.Password, Username = _.Username, Role = _.Role };
            PublicObjects.Image = _.Photo;
            if (_ == null)
            {
                return HttpNotFound();
            }
            ViewBag.Role = new SelectList(db.Roles, "ID", "RoleName", _.Role);
            return View(_);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Person _)
        {
            if (ModelState.IsValid)
            {

                db.Entry(_).State = EntityState.Modified;

                if (Request.Form.Get("Chbtn") == "Upload")
                {
                    ViewBag.Role = new SelectList(db.Roles, "ID", "RoleName");
                    PublicObjects.Image = PublicObjects.ConvertToBytes(_.file);
                    return View(new PersonViewModel() { Photo = PublicObjects.Image });
                }
                _.Photo = PublicObjects.Image;
                db.UpdateUsers(_.FName, _.LName, _.Mail, _.Phone, _.Photo, _.Username, _.PersonID);
                return RedirectToAction("Index");
            }
            ViewBag.Role = new SelectList(db.Roles, "ID", "RoleName", _.Role);
            return View(_);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.Persons.Find(id);
            db.Persons.Remove(person);
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
