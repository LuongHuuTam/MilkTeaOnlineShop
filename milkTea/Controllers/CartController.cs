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
                return View(cart);
            }
            else
            {
                List<Cart> allcartInDb = new CartModel().AllCartOfUser(userInDb.Username);
                ViewBag.TotalMoney = new CartModel().TotalMoneyInCart(userInDb.Username);
                return View("CartInDb", allcartInDb);
            }
        }

        //thêm sp vào giỏ hàng
        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            User_Accounts userInDb = HttpContext.Session["user"] as User_Accounts;
            var pro = new CartModel().GetProducts(id);
            if (pro.Quantity == 0)
            {
                return Content("False");
            }
            if (userInDb == null)
            {               
                if (pro != null)
                {
                    GetCart().Add(pro);
                }
                return Content("true");
            }
            else
            {
                var cart = new CartModel().GetCart(userInDb.Username, id);
                if (cart == null)
                {
                    var cartt = new Cart();
                    cartt.Username = userInDb.Username;
                    cartt.ProductId = pro.ProductId;
                    if (new CartModel().AddCartToDb(cartt, 1))
                    {
                        return Content("true");
                    }
                }
                else
                {
                    if (new CartModel().AddCartToDb(cart, 1))
                    {
                        return Content("true");
                    }
                }
            }        
            return Content("false");
        }

        //mua sản phẩm
        [HttpPost]
        public ActionResult BuyNow(int id)
        {
            User_Accounts userInDb = HttpContext.Session["user"] as User_Accounts;
            var pro = new CartModel().GetProducts(id);
            if(pro.Quantity == 0)
            {
                return Content("false");
            }
            else
            {
                if (userInDb == null)
                {
                    if (pro != null)
                    {
                        GetCart().Add(pro);
                    }
                    return Content("/Cart/Index");
                }
                else
                {
                    var cart = new CartModel().GetCart(userInDb.Username, id);
                    if (cart == null)
                    {
                        var cartt = new Cart();
                        cartt.Username = userInDb.Username;
                        cartt.ProductId = pro.ProductId;
                        if (new CartModel().AddCartToDb(cartt, 1))
                        {
                            return Content("/Cart/Index");
                        }
                    }
                    else
                    {
                        if (new CartModel().AddCartToDb(cart, 1))
                        {
                            return Content("/Cart/Index");
                        }
                    }
                }
            }          
            return Content("/Cart/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAmountCard(FormCollection form)
        {
            User_Accounts userInDb = HttpContext.Session["user"] as User_Accounts;
            if (userInDb == null)
            {
                CartModel cart = Session["Cart"] as CartModel;
                //int proId = int.Parse(form["ProductId"]);
                //int amount = int.Parse(form["Amount"]);
                int proId = 1, amount = 1;
                var isId = int.TryParse(form["ProductId"], out proId);
                var isAmount = int.TryParse(form["Amount"], out amount);

                if (isId == true && isAmount == true)
                {
                    if (cart != null)
                    {
                        cart.UpdateAmount(proId, amount);
                    }
                }
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
                if (cart != null)
                {
                    cart.RemoveCartItem(id);
                }              
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