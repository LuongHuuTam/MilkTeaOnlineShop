using milkTea.Models;
using milkTea.Models.ModelController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace milkTea.Controllers
{
    [AuthorizeController]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.Type = ViewBag.Controller = "Admin";
            User_Accounts user = HttpContext.Session["user"] as User_Accounts;
            return View(user);
        }

        public ActionResult ManageAccount(int page=1,int pagesize=6)
        {
            ViewBag.Type = ViewBag.Controller = "Admin";
            var model = new AccountModel().listAllAccount(page,pagesize);
            return View(model);
        }
        [HttpGet]
        public ActionResult CreateAccount()
        {
            ViewBag.Type = ViewBag.Controller = "Admin";
            return View();
        }
        [HttpPost]
        public ActionResult CreateAccount(User_Accounts user)
        {
            ViewBag.Type = ViewBag.Controller = "Admin";
            if(user.Avatar_url==""||user.Avatar_url==null)
            {
                user.Avatar_url = "/Photo/images/default.png";
            }
            if (new AccountModel().CreateAccount(user))
            {
                return RedirectToAction("ManageAccount");
            }
            else
            {
                ModelState.AddModelError("", "Lỗi... Vui lòng kiểm tra lại các thông tin!");
            }
            return View(user);
        }
    }
}