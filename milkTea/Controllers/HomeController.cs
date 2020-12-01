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
        public ActionResult Index()
        {
            ViewBag.Type = ViewBag.Controller = "Customer";
            List<Products_detail> allProducts = new ProductModel().allProducts();
            return View(allProducts);
        }


        public ActionResult AccountProfile()
        {
            ViewBag.Type = ViewBag.Controller = "Customer";
            return View();
        }


    }
}