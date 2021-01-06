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
            User_Accounts userInDb = HttpContext.Session["user"] as User_Accounts;
            if (userInDb != null)
            {
                var product = new ProductModel().allProductsOfAnotherUser(userInDb.Username, page, pagesize);
                return View(product);
            }
            var allProducts = new ProductModel().allProducts(page, pagesize);           
            return View(allProducts);
        }


        public ActionResult Category(int cat = 0, int page = 1, int pagesize = 6)
        {
            ViewBag.cat = cat;
            ViewBag.Controller = "Home";
            ViewBag.Type = "Customer";
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