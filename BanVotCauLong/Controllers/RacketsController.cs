using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanVotCauLong.Models.Entities;
using System.IO;
using PagedList;

namespace BanVotCauLong.Controllers
{
    public class RacketsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Rackets
        public ActionResult Index(string sO,string searchString,int? idb,int? idp,int? page)
        {
            //quyenf truy cập
            if (Session["admin"] == null)
            {
                return RedirectToAction("Show", "Users");
            }

            int pageSize = 3;          
            int pageNumber = (page ?? 1);

            var rackets = db.Rackets.Include(r => r.Brand);
            if (!String.IsNullOrEmpty(searchString)) // kiểm tra chuỗi tìm kiếm có rỗng/null hay không
            {
                rackets = rackets.Where(s => s.NameRacket.Contains(searchString)); //lọc theo chuỗi tìm kiếm
            }
            var rackets2 = rackets;
            if (sO == "DP")
            {
                rackets = rackets.OrderBy(r => r.IdBrand);
                rackets = rackets.OrderByDescending(r => r.Price);
            }
            else if (sO == "P")
            {
                rackets = rackets.OrderBy(r => r.Price);
            }
            else
            {
                rackets = rackets.OrderBy(r => r.IdBrand);
            }           
            if (idb!=null)    rackets = rackets.Where(x => x.IdBrand == idb);
            else if (idp != null)
            {
                if (idp == 1)
                {
                    return View(rackets.Where(x => x.Price < 100000).ToPagedList(pageNumber, pageSize));
                }
                else if (idp == 2)
                {
                     rackets2 = rackets2.Where(x => x.Price < 200000);
                    rackets = rackets.Where(x => x.Price < 100000);
                    var rackets3 = rackets2.Except(rackets);                   
                    if (sO == "DP")
                    {
                        rackets3 = rackets3.OrderByDescending(r => r.Price);
                    }
                    else if (sO == "P")
                    {
                        rackets3 = rackets3.OrderBy(r => r.Price);
                    }
                    else
                    {
                        rackets3 = rackets3.OrderBy(r => r.IdBrand);
                    }
                    return View(rackets3.ToPagedList(pageNumber, pageSize));
                }
                else if (idp == 3)
                {
                     rackets2 = rackets2.Where(x => x.Price < 500000);
                    rackets = rackets.Where(x => x.Price < 200000);
                    var rackets3 = rackets2.Except(rackets);
                    if (sO == "DP")
                    {
                        rackets3 = rackets3.OrderByDescending(r => r.Price);
                    }
                    else if (sO == "P")
                    {
                        rackets3 = rackets3.OrderBy(r => r.Price);
                    }
                    else
                    {
                        rackets3 = rackets3.OrderBy(r => r.IdBrand);
                    }

                    return View(rackets3.ToPagedList(pageNumber, pageSize));
                }
                else if (idp == 4)
                {
                     rackets2 = rackets2.Where(x => x.Price < 1000000);
                    rackets = rackets.Where(x => x.Price < 500000);
                    var rackets3 = rackets2.Except(rackets);
                    if (sO == "DP")
                    {
                        rackets3 = rackets3.OrderByDescending(r => r.Price);
                    }
                    else if (sO == "P")
                    {
                        rackets3 = rackets3.OrderBy(r => r.Price);
                    }
                    else
                    {
                        rackets3 = rackets3.OrderBy(r => r.IdBrand);
                    }
                    return View(rackets3.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    return View(rackets.Where(x => x.Price >= 1000000).ToPagedList(pageNumber, pageSize));
                }
            }
            
            return View(rackets.ToPagedList(pageNumber, pageSize));
        }

        // GET: Rackets/Details/5
        public ActionResult Details(int? id,bool? re)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Show", "Users");
            }

            foreach (var item in db.Admins)
            {
                if (Session["username"]==null ||(int)Session["username"] != item.IdAdmin)
                {
                    return RedirectToAction("Show", "Users");
                }
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Racket racket = db.Rackets.Find(id);
            if (racket == null)
            {
                return HttpNotFound();
            }
            if(re==false)
            {
                ViewBag.re = true;
            }
            return View(racket);
        }

        // GET: Rackets/Create
        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Show", "Users");
            }

            foreach (var item in db.Admins)
            {
                if (Session["username"]==null ||(int)Session["username"] != item.IdAdmin)
                {
                    return RedirectToAction("Show", "Users");
                }
            }
            ViewBag.IdBrand = new SelectList(db.Brands, "IdBrand", "NameBrand");
            return View();
        }

        // POST: Rackets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdRacket,NameRacket,IdBrand,Quantity,Price,ImageLink")] Racket racket, HttpPostedFileBase FileUpload)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/HinhAnh"), Path.GetFileName(FileUpload.FileName));

                FileUpload.SaveAs(path);

                racket.ImageLink = FileUpload.FileName;
                db.Rackets.Add(racket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdBrand = new SelectList(db.Brands, "IdBrand", "NameBrand", racket.IdBrand);
            return View(racket);
        }

        // GET: Rackets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Show", "Users");
            }

            foreach (var item in db.Admins)
            {
                if (Session["username"]==null ||(int)Session["username"] != item.IdAdmin)
                {
                    return RedirectToAction("Show", "Users");
                }
            }
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

        // POST: Rackets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdRacket,NameRacket,IdBrand,Quantity,Price,ImageLink")] Racket racket, HttpPostedFileBase FileUpload)
        {
            if (ModelState.IsValid)
            {
                if (FileUpload != null)
                {
                    string path = Path.Combine(Server.MapPath("~/HinhAnh"), Path.GetFileName(FileUpload.FileName));

                    FileUpload.SaveAs(path);

                    racket.ImageLink = FileUpload.FileName;
                }
                db.Entry(racket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdBrand = new SelectList(db.Brands, "IdBrand", "NameBrand", racket.IdBrand);
            return View(racket);
        }

        // GET: Rackets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Show", "Users");
            }

            foreach (var item in db.Admins)
            {
                if (Session["username"]==null ||(int)Session["username"] != item.IdAdmin)
                {
                    return RedirectToAction("Show", "Users");
                }
            }
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

        // POST: Rackets/Delete/5
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
        [HttpPost]
        public ActionResult SearchByBrand()
        {
            string id = Request.Form["check"];
            if (!Request.UrlReferrer.ToString().Contains("Rackets")) return RedirectToAction("Show","Users", new { b = id });
            return RedirectToAction("Index", "Rackets", new { idb=id });
        }
        
        [HttpPost]
        public ActionResult SearchByPrice()
        {
            string id = Request.Form["optionsRadios"];
            bool l = Request.UrlReferrer.ToString().Contains("Rackets");
            if (l==false) return RedirectToAction("Show", "Users", new { p = id });
            return RedirectToAction("Index","Rackets", new { idp = id });
            //return RedirectToAction("Index", "Rackets", new { idp = 1 });
        }
       
    }
}
