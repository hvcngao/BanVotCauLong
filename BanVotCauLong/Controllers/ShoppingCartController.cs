using BanVotCauLong.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanVotCauLong.Controllers
{
    public class ShoppingCartController : Controller
    {
        private Model1 db = new Model1();
        // GET: ShoppingCart

        public ActionResult Add(int id)
        {
            ShoppingCart cart = (ShoppingCart)Session["cart"];

            if (cart == null)
            {
                cart = new ShoppingCart();
            }
            //truy can CSDL
            Racket rac = db.Rackets.Find(id);
            cart.InsertItem(rac.NameRacket, rac.IdRacket, (long)rac.Price,rac.ImageLink);
            Session["cart"] = cart;
            int i=(int) Session["gh"];
             i++; Session["gh"] = i;
            return Redirect(Request.UrlReferrer.ToString());//cai duong dan nao goi den no,  thi no tro lai
        }
        public ActionResult Sub(int id)
        {
            ShoppingCart cart = (ShoppingCart)Session["cart"];

            if (cart == null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }          
            
            cart.SubtractItem(id);
            Session["cart"] = cart;
            int i = (int)Session["gh"];
            i--; Session["gh"] = i;
            return Redirect(Request.UrlReferrer.ToString());//cai duong dan nao goi den no,  thi no tro lai
        }
        public ActionResult Cart()
        {
            return View();
        }
        public ActionResult ToPay()
        {
            if (Session["username"] == null) return RedirectToAction("Login", "Users");
            return View();
        }
        [HttpPost]
        public ActionResult ToPay2(User user)
        {
            int idk = (int)Session["username"];
            
            if (ModelState.IsValid)
            {
                var orders = db.Orders.Where(x => x.IdUser == idk).OrderByDescending(x=>x.Date);
                user.IdUser = idk;
                Order or = new Order();
                or.IdUser = user.IdUser;
                or.Date = DateTime.Now;
                or.IdOrder = db.Orders.Count() +1;

                ShoppingCart cart = (ShoppingCart)Session["cart"];
                List<Item> lst = cart.lst;
                or.TotalMonney = (long)cart.GetTotalMoney();
                or.User = db.Users.Find(or.IdUser);
                db.Orders.Add(or);
                //db.Entry(or).State = EntityState.Modified;
                //db.SaveChanges();
                foreach (var it in lst)
                {
                    OrderDetail ord = new OrderDetail();
                    ord.IdOrder = or.IdOrder;
                    ord.IdRacket = it.id;
                    ord.Quantity = it.amount;
                    ord.Money = it.amount * it.price;
                    db.OrderDetails.Add(ord);
                }
                db.SaveChanges();
                Session["cart"] = new ShoppingCart();
                Session["gh"] = 0;
                return View(orders.ToList());
            }            
            return RedirectToAction("ToPay");
        }

        public ActionResult Details(int id)
        {
            var ord = db.OrderDetails.Where(x => x.IdOrder == id);
            return View(ord.ToList());
        }
    }
}