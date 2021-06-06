using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TipsTravels_System.Models;
using TipsTravels_System.ViewModel;

namespace TipsTravels_System.Controllers
{
    public class PostsController : Controller
    {
        private TripsDBEntities db = new TripsDBEntities();

        // GET: Posts
        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.Person).Where(_ => _.Person.Role == 2);
            return View(posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.PersonID = new SelectList(db.Persons.ToList().Where(_ => _.Role == 2), "PersonID", "FName");
            return View(new Post());
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Get("Chbtn") == "Upload")
                {
                    ViewBag.PersonID = new SelectList(db.Persons.ToList().Where(_ => _.Role == 2), "PersonID", "FName");
                    PublicObjects.Image = PublicObjects.ConvertToBytes(post.file);
                    return View(new Post() { TripImage = PublicObjects.Image });
                }
                post.TripImage = PublicObjects.Image;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PersonID = new SelectList(db.Persons.ToList().Where(_ => _.Role == 2), "PersonID", "FName");
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            PublicObjects.Image = post.TripImage;
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonID = new SelectList(db.Persons.ToList().Where(_ => _.Role == 2), "PersonID", "FName");
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostID,PersonID,TripTitle,TripDetails,PostDate,TripDate,TripDestination,TripImage,TripPrice,IsApproved")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                if (Request.Form.Get("Chbtn") == "Upload")
                {
                    ViewBag.PersonID = new SelectList(db.Persons.ToList().Where(_ => _.Role == 2), "PersonID", "FName");
                    PublicObjects.Image = PublicObjects.ConvertToBytes(post.file);
                    return View(new Post() { TripImage = PublicObjects.Image });
                }
                post.TripImage = PublicObjects.Image;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonID = new SelectList(db.Persons.ToList().Where(_ => _.Role == 2), "PersonID", "FName");
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
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
