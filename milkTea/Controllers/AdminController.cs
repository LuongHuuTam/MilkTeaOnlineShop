using milkTea.Models;
using milkTea.Models.ModelController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using milkTea.Assets;

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

        public ActionResult ManageAccount(string search = "", int page = 1, int pagesize = 8)
        {
            ViewBag.Type = ViewBag.Controller = "Admin";
            ViewBag.search = search;
            if (search == "" || search == null)
            {
                var model = new AccountModel().listAllAccount(page, pagesize);
                return View(model);
            }
            else
            {

                var model = new AccountModel().listAllAccount(search, page, pagesize);
                return View(model);
            }
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
            if (user.Avatar_url == "" || user.Avatar_url == null)
            {
                user.Avatar_url = "/Photo/images/default.png";
            }
            try
            {
                user.Password = Encryptor.MD5Hash(user.Password);
            }
            catch
            {
                ModelState.AddModelError("", "Lỗi... Vui lòng kiểm tra lại các thông tin!");
                return View(user);
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


        [HttpPost]
        public ActionResult uppdateAvatar(string username, string ava_url)
        {
            User_Accounts user = new AccountModel().getAccount(username);
            user.Avatar_url = ava_url;
            if (new AccountModel().updateAccount(user))
            {
                return Content("true");
            }
            return Content("false");
        }
        [HttpPost]
        public ActionResult uppdatePass(string username, string oldPass, string newPass, string rePass)
        {
            User_Accounts user = new AccountModel().getAccount(username);
            oldPass = Encryptor.MD5Hash(oldPass);

            if ((new AccountModel().login(username, oldPass)).Username != username)
                return Content("mk khong dung");
            if (newPass != rePass || newPass == "" || rePass == "")
                return Content("nhap lai mk sai");
            user.Password = Encryptor.MD5Hash(newPass);
            if (new AccountModel().updateAccount(user))
            {
                return Content("true");
            }
            return Content("false");
        }
        [HttpPost]
        public ActionResult uppdateInfo(string username, string newFn, string newLn, string newE, string newA, string newP)
        {
            User_Accounts user = new AccountModel().getAccount(username);
            user.FirstName = newFn;
            user.LastName = newLn;
            user.PhoneNumber = newP;
            user.Address = newA;
            user.Email = newE;
            if (new AccountModel().updateAccount(user))
            {
                return Content("true");
            }
            return Content("false");
        }
        [HttpGet]
        public ActionResult ManageCategory(int cat = 0, int page = 1, int pagesize = 8)
        {
            int a;
            
            if (cat == 0)
            {
                ViewBag.cat = cat + 1;
                a = 2;
            }
            else
            {
                ViewBag.cat = cat;
                a = cat + 1;
            }
            ViewBag.data = a;     
            ViewBag.Type = ViewBag.Controller = "Admin";
            var model = new ProductModel().getProByCat(cat, page, pagesize);
            return View(model);
        }
        [HttpPost]
        public ActionResult addCategory(string catName)
        {
            int res = new ProductModel().addCategory(catName);
            return Content(res.ToString());
        }

    }
}