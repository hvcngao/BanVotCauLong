using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanVotCauLong.Models.Entities;

namespace BanVotCauLong.Controllers.WebRacket
{
    public class RacketController : Controller
    {
        private Model1 db = new Model1();

        // GET: Racket
        public ActionResult Index()
        {
            var rackets = db.Rackets.Include(r => r.Brand);
            return View(rackets.ToList());
        }

        // GET: Racket/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Racket racket = db.Rackets.Find(id);
            if (racket == null)
            {
                return HttpNotFound();
            }
            return View(racket);
        }

        // GET: Racket/Create
        public ActionResult Create()
        {
            ViewBag.IdBrand = new SelectList(db.Brands, "IdBrand", "NameBrand");
            return View();
        }

        // POST: Racket/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdRacket,NameRacket,IdBrand,Quantity,Price,ImageLink")] Racket racket)
        {
            if (ModelState.IsValid)
            {
                db.Rackets.Add(racket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdBrand = new SelectList(db.Brands, "IdBrand", "NameBrand", racket.IdBrand);
            return View(racket);
        }

        // GET: Racket/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Racket racket = db.Rackets.Find(id);
            if (racket == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdBrand = new SelectList(db.Brands, "IdBrand", "NameBrand", racket.IdBrand);
            return View(racket);
        }

        // POST: Racket/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdRacket,NameRacket,IdBrand,Quantity,Price,ImageLink")] Racket racket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(racket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdBrand = new SelectList(db.Brands, "IdBrand", "NameBrand", racket.IdBrand);
            return View(racket);
        }

        // GET: Racket/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Racket racket = db.Rackets.Find(id);
            if (racket == null)
            {
                return HttpNotFound();
            }
            return View(racket);
        }

        // POST: Racket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Racket racket = db.Rackets.Find(id);
            db.Rackets.Remove(racket);
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
        public ActionResult Left()
        {
            var brand = db.Brands;
            return View(brand.ToList());
        }
    }
}
