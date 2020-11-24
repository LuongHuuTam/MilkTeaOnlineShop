using milkTea.Models.ModelController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace milkTea.Controllers
{
    [AuthorizeController]
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            ViewBag.Type = ViewBag.Controller = "Customer";
            return View();
        }
    }
}