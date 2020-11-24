﻿using milkTea.Assets;
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
            if(res!=null)
            {
                Session["user"] = res;
                if (res.Type == 1)
                    return RedirectToAction("index", "admin");
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
    }
}