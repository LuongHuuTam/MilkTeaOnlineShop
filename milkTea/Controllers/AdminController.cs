using milkTea.Models;
using milkTea.Models.ModelController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace milkTea.Controllers
{
    [AuthorizeController]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.Type = "Admin";
            ViewBag.Controller = "Admin";
            User_Accounts user = HttpContext.Session["user"] as User_Accounts;
            return View(user);
        }
    }
}