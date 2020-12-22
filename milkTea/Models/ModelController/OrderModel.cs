using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace milkTea.Models.ModelController
{
    public class OrderModel
    {
        private MilkTeaModel _context = null;
        public OrderModel()
        {
            _context = new MilkTeaModel();
        }
        public List<Cart> carts { get; set; }
        public User_Accounts user { get; set; }
        public Orders_Detail detail { get; set; }
        public Order orders { get; set; }
        //list đơn hàng
        public List<Orders_Detail> AllOrderMenusOfUser(string username)
        {
            var orderMenus = (from i in _context.Orders_Detail
                              where i.Customer == username
                              select i).ToList();
            return orderMenus;
        }

        //public List<Order> AllOrderDetail(int id)
        //{
        //    var orderDetail = (from i in _context.Orders
        //                       where i.Orders_Detail == id
        //                       select i).ToList();
        //    return orderDetail;
        //}

        //thêm đơn hàng vô db
        public bool AddOrderMenuToDb(Order order, int amount)
        {
            try
            {
                order.Amount = amount;
                _context.Orders.Add(order);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //thêm chi tiết đơn hàng
        public bool AddOrderDetail(Orders_Detail ordersDetail)
        {
            try
            {
                _context.Orders_Detail.Add(ordersDetail);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //public float TotalMoneyInCart()
        //{
        //    var total = _context.Orders.Sum(t => (double?)t.Products_Detail.Price * t.Amount) ?? 0f;
        //    return (float)total;
        //}

        public Order GetOrder(int id)
        {
            var order = _context.Orders.SingleOrDefault(o => o.OrderId == id);
            return order;
        }

        //public Orders_Detail GetOrders_Detail(int id)
        //{
        //    var orderDetail = (from i in _context.Orders_Detail
        //                       join o in _context.Orders
        //                       on id equals o.OrderId
        //                       select i).SingleOrDefault();
        //    return orderDetail;
        //}

        public IEnumerable<Order> GetOrdersByStatus(int id, int status, int page, int pagesize)
        {
            var order = (from i in _context.Orders
                         where i.Status == status
                         where i.Orders_Detail == id
                         orderby i.OrderId
                         select i).ToPagedList(page, pagesize);
            return order;
        }

        public bool UpdateOrder(Order order)
        {
            try
            {
                _context.Orders.AddOrUpdate(order);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int GetOrderIdByOrder_Detail(int id)
        {
            var orderId = _context.Orders.SingleOrDefault(o => o.OrderId == id);
            return orderId.Orders_Detail;
        }
    }
}