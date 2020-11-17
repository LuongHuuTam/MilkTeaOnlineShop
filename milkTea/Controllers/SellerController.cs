using milkTea.Models.ModelController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace milkTea.Controllers
{
    [AuthorizeController]
    public class SellerController : Controller
    {
        // GET: Seller
        public ActionResult Index()
        {
            ViewBag.Type = ViewBag.Controller = "Seller";
            return View();
        }
    }
}