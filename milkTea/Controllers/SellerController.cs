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
        //Cửa hàng
        [HttpGet]
        public ActionResult Shop()
        {
            ViewBag.Type = "Seller";
            ViewBag.Controller = "Home";
            var seller = (User_Accounts)Session["user"];
            var list = new ProductModel().GetProductBySeller(seller);
            return View(list);
        }
        //Đơn hàng
        [HttpGet]
        public ActionResult Order()
        {
            ViewBag.Type = "Seller";
            ViewBag.Controller = "Home";
            var seller = (User_Accounts)Session["user"];
            var list = new OrderModel().GetOrderBySeller(seller);
            return View(list);
        }
        [HttpGet]
        public ActionResult OrderDetail(int? orderId)
        {
            ViewBag.Type = "Seller";
            ViewBag.Controller = "Home";
            var order = new OrderModel().GetOrderById(orderId);
            return View(order);
        }
        public ActionResult Confirm(int? orderId, byte status)
        {
            ViewBag.Type = ViewBag.Controller = "Seller";
            var orderModel = new OrderModel();
            var order = orderModel.GetOrderById(orderId);
            var pro = new ProductModel().GetProductByOrder(order);
            bool i = false;
            switch (status)
            {
                case 0:
                    i = orderModel.Update(orderId, 1);
                    break;
                case 1:
                    i = orderModel.Update(orderId, 2);
                    pro.Quantity -= order.Amount;
                    i = new ProductModel().UpdateProduct(pro);
                    break;
            }
            if (i)
            {
                return RedirectToAction("Order");
            }
            else
            {
                return RedirectToAction("OrderDetail");
            }
        }
        //Thêm sản phẩm
        [HttpGet]
        public ActionResult AddProduct()
        {
            ViewBag.Type = "Seller";
            ViewBag.Controller = "Home";
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(Products_Detail pro)
        {
            ViewBag.Type = "Seller";
            ViewBag.Controller = "Home";
            var seller = (User_Accounts)Session["user"];
            if (pro.Imgage_url == "" || pro.Imgage_url == null)
            {
                pro.Imgage_url = "/Photo/images/drinkDefault.png";
            }
            pro.Seller = seller.Username;
            if (ModelState.IsValid)
            {
                var i = new ProductModel().AddProduct(pro);
                if (i)
                {
                    return RedirectToAction("Shop");
                }
                else
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi");
                }
            }
            else
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi");
            }
            return View("AddProduct");
        }        
        //Sửa sản phẩm
        [HttpGet]
        public ActionResult UpdateProduct(int? productId)
        {
            ViewBag.Type = "Seller";
            ViewBag.Controller = "Home";
            var product = new ProductModel().GetProductById(productId);
            return View(product);
        }
        [HttpPost]
        public ActionResult UpdateProduct(Products_Detail product)
        {
            ViewBag.Type = "Seller";
            ViewBag.Controller = "Home";
            if (ModelState.IsValid)
            {
                var i = new ProductModel().UpdateProduct(product);
                if (i)
                {
                    return RedirectToAction("Shop");
                }
                else
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi");
                }
            }
            else
            {

                ModelState.AddModelError("", "Đã xảy ra lỗi");
            }
            return RedirectToAction("UpdateProduct", new { @productId = product.ProductId });
        }
        public ActionResult UpdateImg(string proId, string img_url)
        {
            var product = new ProductModel().GetProductById(Int32.Parse(proId));
            product.Imgage_url = img_url;
            if (new ProductModel().UpdateProduct(product))
            {
                return Content("true");
            }
            return Content("false");
        }
        //Xoá Sản phẩm
        public ActionResult DeleteProduct(int productId)
        {
            var i = new ProductModel().DeleteProduct(productId);
            return RedirectToAction("Shop", "Seller");
        }
    }
}
