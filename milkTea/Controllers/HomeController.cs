using milkTea.Models;
using milkTea.Models.ModelController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace milkTea.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(int page = 1, int pagesize = 6)
        {
            ViewBag.Name = "Trang sản phẩm";
            ViewBag.Controller = "Home";
            ViewBag.Type = "Customer";
            var allProducts = new ProductModel().allProducts(page, pagesize);
            return View(allProducts);
        }

        //[HttpGet]
        //public ActionResult AccountProfile()
        //{
        //    ViewBag.Type = ViewBag.Controller = "Customer";
        //    User_Accounts user = HttpContext.Session["user"] as User_Accounts;
        //    return View(user);
        //}

        //[HttpPost]
        //public ActionResult AccountProfile(User_Accounts user)
        //{
        //    ViewBag.Type = ViewBag.Controller = "Customer";
        //    User_Accounts userInDb = HttpContext.Session["user"] as User_Accounts;
        //    if (user.FirstName == null || user.LastName == null || user.PhoneNumber == null || user.Email == null || user.Address == null)
        //    {
        //        user.FirstName = "";
        //        user.LastName = "";
        //        user.PhoneNumber = "";
        //        user.Email = "";
        //        user.Address = "";
        //    }
        //    userInDb.FirstName = user.FirstName;
        //    userInDb.LastName = user.LastName;
        //    userInDb.PhoneNumber = user.PhoneNumber;
        //    userInDb.Email = user.Email;
        //    userInDb.Address = user.Address;
        //    if (new AccountModel().updateAccount(userInDb))
        //    {
        //        return View(userInDb);
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Lỗi");
        //    }    

        //    return View();
        //}

        //[HttpPost]
        //public ActionResult UpdateAvatar(string username, string ava_url)
        //{
        //    User_Accounts user = new AccountModel().getAccount(username);
        //    user.Avatar_url = ava_url;
        //    if (new AccountModel().updateAccount(user))
        //    {
        //        return Content("true");
        //    }    

        //    return Content("false");
        //}

        public ActionResult Category(int cat = 0, int page = 1, int pagesize = 6)
        {
            ViewBag.cat = cat;
            ViewBag.Type = ViewBag.Controller = "Customer";
            var model = new ProductModel().getProByCat(cat, page, pagesize);
            ViewBag.Name = new ProductModel().getNameByCatID(cat);
            return View("Index", model);
        }
        public ActionResult ProductsOfSearch(string search = "", int page = 1, int pagesize = 6)
        {
            ViewBag.Name = "Kết quả tìm kiếm cho " + search;
            ViewBag.Type = ViewBag.Controller = "Customer";
            ViewBag.search = search;
            if (search == null || search == "")
            {
                return View("Index");
            }
            else
            {
                var pro = new ProductModel().searchProducts(search, page, pagesize);
                return View("Index", pro);
            }
        }
    }
}