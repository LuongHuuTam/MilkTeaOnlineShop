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

        public bool UpdateQuantityOfProduct(int id, int amount)
        {
            try
            {
                var product = _context.Products_Detail.SingleOrDefault(p => p.ProductId == id);
                product.Quantity -= amount;
                _context.Products_Detail.AddOrUpdate(product);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckProduct(string username, int proId)
        {
            var productOfUser = _context.Products_Detail
                .Where(p => p.Seller == username)
                .SingleOrDefault(p => p.ProductId == proId);
            if (productOfUser == null)
            {
                return false;
            }
            return true;
        }
        
        
        
        //-----------------------------------


        public List<Ship_Method> allShip()
        {
            return _context.Ship_Method.ToList();
        }
        public Ship_Method GetShipMethodById(int? id)
        {
            return _context.Ship_Method.Find(id);
        }
        public Orders_Detail GetOrderDetailById(int? Id)
        {
            return _context.Orders_Detail.Find(Id);
        }
        //public int AddOrderDetail(Orders_Detail orderDt)
        //{

        //    _context.Orders_Detail.Add(orderDt);
        //    _context.SaveChanges();
        //    return orderDt.OrderDetailId;

        //}
        public Order GetOrderById(int? Id)
        {
            return _context.Orders.Find(Id);
        }
        public IEnumerable<Order> GetOrderByCustomer(User_Accounts acc)
        {
            List<Orders_Detail> listOD = new List<Orders_Detail>();
            List<Order> listO = new List<Order>();
            List<Order> listResult = new List<Order>();
            listOD = _context.Orders_Detail.Where(x => x.Customer == acc.Username).ToList();
            foreach (var od in listOD)
            {
                listO = _context.Orders.Where(x => x.Orders_Detail == od.OrderDetailId).ToList();
                foreach (var o in listO)
                {
                    listResult.Add(o);
                }
            }
            return listResult;
        }
        public IEnumerable<Order> GetOrderByProduct(Products_Detail pro)
        {
            return _context.Orders.Where(x => x.ProductId == pro.ProductId).ToList();
        }
        public IEnumerable<Order> GetOrderBySeller(User_Accounts acc)
        {
            var listOrder = new List<Order>();
            //lấy danh sách sp của seller
            var listProduct = new ProductModel().GetProductBySeller(acc);
            //Tương ứng mỗi sp, tìm order có proId trùng với Id sản phẩm, nếu có thì lưu vào listOrder
            foreach (var product in listProduct)
            {
                var list = new OrderModel().GetOrderByProduct(product);
                if (list != null)
                {
                    foreach (var order in list)
                    {
                        listOrder.Add(order);
                    }
                }
            }
            return listOrder;
        }
        public bool AddListOrders(List<Order> list)
        {
            try
            {
                foreach (var i in list)
                {
                    _context.Orders.Add(i);
                }
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(int? OrderId, byte status)
        {
            try
            {
                Order order = _context.Orders.Find(OrderId);
                order.Status = status;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
