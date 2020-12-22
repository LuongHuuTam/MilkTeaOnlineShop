using milkTea.Assets;
using milkTea.Models;
using milkTea.Models.ModelController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace milkTea.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(User_Accounts us)
        {
            User_Accounts res = new AccountModel().login(us.Username, Encryptor.MD5Hash(us.Password));
            if (res != null)
            {
                Session["user"] = res;
                if (res.Type == 1)
                    return RedirectToAction("index", "admin");
                CartModel cart = Session["Cart"] as CartModel;
                //lưu session cart vào db
                if (cart != null)
                {
                    var cartInDb = new Cart();
                    foreach (var item in cart.Items)
                    {
                        cartInDb.Username = res.Username;
                        cartInDb.ProductId = item.productsInCart.ProductId;
                        if (new CartModel().AddCartSessionToDb(cartInDb, item.amountInCart))
                        { }
                    }
                    Session["Cart"] = null;
                }
                return RedirectToAction("index", "home");
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            }
            return View(us);
        }
        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("index", "home");
        }
        [HttpGet]
        public ActionResult registerAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult registerAccount(User_Accounts model, string rePass)
        {
            if (model.Password != rePass || model.Password == null)
            {
                ModelState.AddModelError("", "Hai mật khẩu không trùng khớp");
                return View(model);
            }
            model.Password = Encryptor.MD5Hash(model.Password);
            model.Type = 3;
            model.Avatar_url = "/Photo/images/default.png";
            if (new AccountModel().CreateAccount(model))
            {
                return View("index");
            }
            ModelState.AddModelError("", "Lỗi... Hãy thử lại");
            return View(model);
        }
    }
}