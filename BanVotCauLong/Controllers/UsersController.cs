using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanVotCauLong.Models.Entities;
using PagedList;

namespace BanVotCauLong.Controllers
{
    public class UsersController : Controller
    {
        private Model1 db = new Model1();

        public ActionResult Show2()
        {
            return View();
        }
        // GET: Users
        public ActionResult Show(string sO, string searchString, int? b, int? p, int? page)
        {          
            // 2. Nếu page = null thì đặt lại là 1.
            if (page == null) page = 1;

            // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo LinkID mới có thể phân trang.           
            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 3;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // 5. Trả về các Link được phân trang theo kích thước và số trang.
            //return View(links.ToPagedList(pageNumber, pageSize));

            if (Session["gh"] ==null)     Session["gh"] = 0;
            var rackets = db.Rackets.Include(r => r.Brand);
            if (!String.IsNullOrEmpty(searchString)) // kiểm tra chuỗi tìm kiếm có rỗng/null hay không
            {
                rackets = rackets.Where(s => s.NameRacket.Contains(searchString)); //lọc theo chuỗi tìm kiếm
            }
            if (sO == "DP")
            {
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
            
            if (b != null) rackets = rackets.Where(x => x.IdBrand == b);
            else if (p != null)
            {
                if (p == 1)
                {
                    return View(rackets.Where(x => x.Price < 100000).ToPagedList(pageNumber, pageSize));
                }
                else if (p == 2)
                {
                    var rackets2 = rackets.Where(x => x.Price < 200000);
                    rackets = rackets.Where(x => x.Price < 100000);
                    var rackets3 = rackets2.Except(rackets).OrderBy(x => x.IdBrand);


                    return View(rackets3.ToPagedList(pageNumber, pageSize));
                }
                else if (p == 3)
                {
                    var rackets2 = rackets.Where(x => x.Price < 500000);
                    rackets = rackets.Where(x => x.Price < 200000);
                    var rackets3 = rackets2.Except(rackets).OrderBy(x => x.IdBrand);

                    return View(rackets3.ToPagedList(pageNumber, pageSize));
                }
                else if (p == 4)
                {
                    var rackets2 = rackets.Where(x => x.Price < 1000000);
                    rackets = rackets.Where(x => x.Price < 500000);
                    var rackets3 = rackets2.Except(rackets).OrderBy(x => x.IdBrand);

                    return View(rackets3.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    return View(rackets.Where(x => x.Price >= 1000000).OrderBy(r => r.IdBrand).ToPagedList(pageNumber, pageSize));
                }
            }
            return View(rackets.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Show", "Users");
            }
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Show", "Users");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Show", "Users");
            }
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdUser,NameUser,Email,PassWord,PhoneNumber,Address")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Show", "Users");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUser,NameUser,Email,PassWord,PhoneNumber,Address")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Show", "Users");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            string us = Request.Form["us"];
            string mk = Request.Form["mk"];

            foreach (var item in db.Admins)
            {
                if (us.Equals(item.Username.ToString()) && mk.Equals(item.Password.ToString()))
                {
                    ViewBag.er = null;
                    Session["admin"] = item.IdAdmin;
                    Session["nameadmin"] = item.NameAdmin;
                    //int k = (int)Session["gh"];
                    //if (k > 0) return RedirectToAction("ToPay", "ShoppingCart");
                    return RedirectToAction("Show", "Admins");

                }
            }

            foreach (var item in db.Users)
            {
                if (us.Equals(item.UserName.ToString()) && mk.Equals(item.PassWord.ToString()))
                {
                    ViewBag.er = null;
                    Session["username"] = item.IdUser;
                    Session["nameuser"] = item.NameUser;
                    int k = (int)Session["gh"];
                    if (k > 0) return RedirectToAction("ToPay", "ShoppingCart");
                    return RedirectToAction("Show", "Users");                   
                }
            }
            ViewBag.er = "Sai tai Khoan Hoac Mat Khau";

            return View();
        }
        public ActionResult Logout()
        {
            Session["username"] = null;
            Session["nameuser"] = null;
            Session["admin"] = null;
            Session["nameadmin"] = null;
            return RedirectToAction("Show");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user)
        {
            var users = db.Users;
            string us = Request.Form["us"];
            string mk = Request.Form["mk"];
            string cfmk = Request.Form["cfmk"];
            if (!mk.Equals(cfmk))
            {
                ViewBag.errr = "Xác nhận mật khẩu không đúng với mật khẩu";
                return View();
            }
            foreach(var item in users)
            {
                if (us.Equals(item.UserName.ToString()))
                {
                    ViewBag.errr = "Tên tài khoản đã tồn tại";
                    return View();
                }
            }
            user.UserName = us;
            user.PassWord = mk;
            user.IdUser = db.Users.Count() + 1;
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Login");
        }
       
    }
}
