﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace milkTea.Models.ModelController
{
    public class AuthorizeController : ActionFilterAttribute
    {
        MilkTeaModel db = new MilkTeaModel();
        //phương thức thực thi khi action được gọi
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            User_Accounts tbus = HttpContext.Current.Session["user"] as User_Accounts;
            //nếu session=null thì trả về trang đăng nhập
            if (tbus == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
            }
            //session != null
            else
            {
                string permission="";
                switch (tbus.Type)
                {
                    case 1:
                        permission = "Admin";
                        break;
                    case 2:
                        permission = "Seller";
                        break;
                    case 3:
                        permission = "Customer";
                        break;
                }
                //Lấy tên controller và action
                //string actionName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "-" + filterContext.ActionDescriptor.ActionName;

                //Lấy tên Controller
                string ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                //nếu tên controler không có trong mảng quyền của user thì trả về trang đăng nhập
                if (ControllerName!=permission)
                {
                    filterContext.Result = new RedirectResult("~/Login/Index");
                }
            }
        }
    }
}