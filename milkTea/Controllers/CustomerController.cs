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
            if (user.FirstName == null || user.LastName == null ||
                user.PhoneNumber == null || user.Email == null || user.Address == null)
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


        public ActionResult checkProduct()
        {
            User_Accounts userInDb = HttpContext.Session["user"] as User_Accounts;
            var allCart = new CartModel().AllCartOfUser(userInDb.Username);
            foreach (var item in allCart)
            {
                var pro = new CartModel().GetProducts(item.ProductId);
                if (new OrderModel().CheckProduct(item.Username, item.ProductId) ||
                    item.Amount > pro.Quantity)
                {
                    return Content("false");
                }
            }
            return Content("/Customer/BuyProducts");
        }

        // mua hàng
        public ActionResult BuyProducts()
        {
            ViewBag.Type = "Customer";
            ViewBag.Controller = "Home";
            User_Accounts userInDb = HttpContext.Session["user"] as User_Accounts;
            var allCart = new CartModel().AllCartOfUser(userInDb.Username);
            var model = new OrderModel
            {
                carts = allCart,
                user = userInDb
            };
            ViewBag.TotalMoney = new CartModel().TotalMoneyInCart();
            return View(model);
        }

        //trang đơn hàng
        public ActionResult OrderMenu()
        {
            ViewBag.Type = "Customer";
            ViewBag.Controller = "Home";
            User_Accounts userInDb = HttpContext.Session["user"] as User_Accounts;
            var orderMenus = new OrderModel().AllOrderMenusOfUser(userInDb.Username);

            return View(orderMenus);
        }

        //đặt hàng
        [HttpPost]
        public ActionResult Order(OrderModel order)
        {
            ViewBag.Type = "Customer";
            ViewBag.Controller = "Home";
            User_Accounts userInDb = HttpContext.Session["user"] as User_Accounts;
            List<Cart> carts = new CartModel().AllCartOfUser(userInDb.Username);
            var orderMenu = new Order();
            var orderDetail = new Orders_Detail();
            orderDetail.Customer = userInDb.Username;
            orderDetail.OrderDate = DateTime.Now;
            orderDetail.Note = order.detail.Note;
            orderDetail.TotalAmount = new CartModel().TotalAmountCartInDB(userInDb.Username);
            orderDetail.TotalMoney = new CartModel().TotalMoneyInCart();
            if (new OrderModel().AddOrderDetail(orderDetail))
            {
                foreach (var item in carts)
                {
                    orderMenu.ProductId = item.ProductId;
                    orderMenu.Orders_Detail = orderDetail.OrderDetailId;
                    //orderMenu.Orders_Detail1.ShipId = order.detail.ShipId;
                    if (new OrderModel().AddOrderMenuToDb(orderMenu, item.Amount) &&
                        new CartModel().DeleteCartFromDb(userInDb.Username, item.ProductId) &&
                        new OrderModel().UpdateQuantityOfProduct(item.ProductId, item.Amount))
                    {
                    }
                }
            }
            return RedirectToAction("OrderMenu");
        }

        public ActionResult OrderDetails(int id, int status = 0, int page = 1, int pagesize = 8)
        {
            ViewBag.Type = "Customer";
            ViewBag.Controller = "Home";
            if (status == 0)
            {
                ViewBag.Data = 1;
            }
            else if (status == 1)
            {
                ViewBag.Data = 2;
            }
            else
            {
                ViewBag.Data = 3;
            }
            var orderDetail = new OrderModel().GetOrdersByStatus(id, status, page, pagesize);
            return View(orderDetail);
        }

        [HttpPost]
        public ActionResult CancelProduct(int id)
        {
            var orderId = new OrderModel().GetOrderIdByOrder_Detail(id);
            var order = new OrderModel().GetOrder(id);
            order.Status = 2;
            if (new OrderModel().UpdateOrder(order))
            {
                return Content("/Customer/OrderDetails/" + orderId.ToString());
            }
            return Content("false");
        }
    }
}