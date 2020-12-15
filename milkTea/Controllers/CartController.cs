using milkTea.Models;
using milkTea.Models.ModelController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace milkTea.Controllers
{
    public class CartController : Controller
    {
        //sử dụng session
       public CartModel GetCart()
       {
            CartModel cart = Session["Cart"] as CartModel;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new CartModel();
                Session["Cart"] = cart;
            }
            return cart;
       }

        //trang giỏ hàng
        public ActionResult Index()
        {
            ViewBag.Type = "Customer";
            ViewBag.Controller = "Home";
            User_Accounts userInDb = HttpContext.Session["user"] as User_Accounts;
            if (userInDb == null)
            {
                CartModel cart = Session["Cart"] as CartModel;                
                //if (Session["Cart"] == null)
                //{
                //    return View(cart);
                //}

                return View(cart);
            }
            else
            {
                List<Cart> cartInDb = new CartModel().AllCartOfUser(userInDb.Username);
                ViewBag.TotalMoney = new CartModel().TotalMoneyInCart();
                return View("CartInDb", cartInDb);
            }
        }

        //trang đơn mua
        public ActionResult Purchase()
        {
            ViewBag.Type = "Customer";
            ViewBag.Controller = "Home";
            User_Accounts userInDb = HttpContext.Session["user"] as User_Accounts;
            if (userInDb == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                List<Cart> cartInDb = new CartModel().AllOrderOfUser(userInDb.Username);
                ViewBag.TotalMoney = new CartModel().TotalMoneyInCart();
                return View(cartInDb);
            }
        }

        //thêm sp vào giỏ hàng
        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            //CartModel cart = Session["Cart"] as CartModel;
            //var pro = new CartModel().GetProducts(id);
            //if (pro != null)
            //{
            //    GetCart().Add(pro);
            //}
            //Session["cart"] = cart;
            Products_detail pro = null;
            User_Accounts userInDb = HttpContext.Session["user"] as User_Accounts;
            if (userInDb == null)
            {
                pro = new CartModel().GetProducts(id);
                if (pro != null)
                {
                    GetCart().Add(pro);
                }
                return Content("true");
            }
            else
            {
                pro = new CartModel().GetProducts(id);
                var cart = new CartModel().GetCart(userInDb.Username, id);
                if (cart == null)
                {
                    var cartt = new Cart();
                    cartt.Username = userInDb.Username;
                    cartt.ProductId = pro.ProductId;
                    cartt.Status = false;
                    if (new CartModel().AddCartToDb(cartt))
                    {
                        return Content("true");
                    }
                }
                else
                {
                    if (new CartModel().AddCartToDb(cart))
                    {
                        return Content("true");
                    }
                }
            }
        
            return Content("false");
        }

        //mua sản phẩm
        public ActionResult BuyNow(int id)
        {
            Products_detail pro = null;
            User_Accounts userInDb = HttpContext.Session["user"] as User_Accounts;
            if (userInDb == null)
            {
                pro = new CartModel().GetProducts(id);
                if (pro != null)
                {
                    GetCart().Add(pro);
                }
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                pro = new CartModel().GetProducts(id);
                var cart = new CartModel().GetCart(userInDb.Username, id);
                if (cart == null)
                {
                    var cartt = new Cart();
                    cartt.Username = userInDb.Username;
                    cartt.ProductId = pro.ProductId;
                    cartt.Status = false;
                    if (new CartModel().AddCartToDb(cartt))
                    {
                        return RedirectToAction("Index", "Cart");
                    }
                }
                else
                {
                    if (new CartModel().AddCartToDb(cart))
                    {
                        return RedirectToAction("Index", "Cart");
                    }
                }               
            }
            return RedirectToAction("Index", "Cart");
        }

        public ActionResult UpdateAmountCard(FormCollection form)
        {
            User_Accounts userInDb = HttpContext.Session["user"] as User_Accounts;
            if (userInDb == null)
            {
                CartModel cart = Session["Cart"] as CartModel;
                int proId = int.Parse(form["ProductId"]);
                int amount = int.Parse(form["Amount"]);
                cart.UpdateAmount(proId, amount);
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                var cart = new CartModel().GetCart(userInDb.Username, int.Parse(form["ProductId"]));
                if (new CartModel().UpdateAmountInCart(cart, int.Parse(form["Amount"])))
                {
                    return RedirectToAction("Index", "Cart");
                }
                return RedirectToAction("Index", "Cart");
            }

        }
        public ActionResult RemoveCart(int id)
        {
            User_Accounts userInDb = HttpContext.Session["user"] as User_Accounts;
            if (userInDb == null)
            {
                CartModel cart = Session["Cart"] as CartModel;
                cart.RemoveCartItem(id);
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                if (new CartModel().DeleteCartFromDb(userInDb.Username, id))
                {
                    return RedirectToAction("Index", "Cart");
                }
                return RedirectToAction("Index", "Cart");
            }
           
        }

        public  PartialViewResult BagCart()
        {            
            int item = 0;
            User_Accounts userInDb = HttpContext.Session["user"] as User_Accounts;
            if (userInDb == null)
            {
                CartModel cart = Session["Cart"] as CartModel;
                if (cart != null)
                {
                    item = cart.TotalAmountInCart();
                }
                ViewBag.numOfCart = item;
            }
            else
            {
                item = new CartModel().TotalAmountCartInDB(userInDb.Username);
                ViewBag.numOfCart = item;
            }           
            return PartialView("BagCart");
        }
    }
}