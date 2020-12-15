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
    public class CustomerController : Controller
    {
        // GET: Customer
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Type = "Customer";
            ViewBag.Controller = "Home";
            User_Accounts user = HttpContext.Session["user"] as User_Accounts;
            return View(user);
        }

        [HttpPost]
        public ActionResult Index(User_Accounts user)
        {
            ViewBag.Type = "Customer";
            ViewBag.Controller = "Home";
            User_Accounts userInDb = HttpContext.Session["user"] as User_Accounts;
            if (user.FirstName == null || user.LastName == null || user.PhoneNumber == null || user.Email == null || user.Address == null)
            {
                user.FirstName = "";
                user.LastName = "";
                user.PhoneNumber = "";
                user.Email = "";
                user.Address = "";
            }
            userInDb.FirstName = user.FirstName;
            userInDb.LastName = user.LastName;
            userInDb.PhoneNumber = user.PhoneNumber;
            userInDb.Email = user.Email;
            userInDb.Address = user.Address;
            if (new AccountModel().updateAccount(userInDb))
            {
                return View(userInDb);
            }
            else
            {
                ModelState.AddModelError("", "Lỗi");
            }

            return View();
        }

        [HttpPost]
        public ActionResult UpdateAvatar(string username, string ava_url)
        {
            User_Accounts user = HttpContext.Session["user"] as User_Accounts;

            user.Avatar_url = ava_url;
            if (new AccountModel().updateAccount(user))
            {
                return Content("true");
            }

            return Content("false");
        }


    }
}