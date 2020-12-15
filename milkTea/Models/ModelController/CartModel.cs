using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace milkTea.Models.ModelController
{
    // thực hiện trong session
    public class CartItem
    {
        public Products_detail productsInCart { get; set; }
        public int amountInCart { get; set; }
    }

    public class CartModel
    {
        private MilkTeaModel _context = null;

        public CartModel()
        {
            _context = new MilkTeaModel();
        }

        public Cart Cart { get; set; }

        List<CartItem> items = new List<CartItem>();

        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }

        public void Add(Products_detail products, int amount = 1)
        {
            var item = items.SingleOrDefault(p => p.productsInCart.ProductId == products.ProductId);
            if (item == null)
            {
                items.Add(new CartItem
                {
                    productsInCart = products,
                    amountInCart = amount
                });

            }
            else
            {
                item.amountInCart += amount;
            }
        }

        public Products_detail GetProducts(int id)
        {
            var pro = _context.Products_detail.SingleOrDefault(p => p.ProductId == id);
            return pro;
        }

        public void UpdateAmount(int id, int amount)
        {
            var item = items.Find(s => s.productsInCart.ProductId == id);
            if (item != null)
            {
                item.amountInCart = amount;
            }
        }

        public double TotalMoney()
        {
            var total = items.Sum(s => s.productsInCart.Price * s.amountInCart);
            return (double)total;
        }
        //xóa giỏ hàng tro
        public void RemoveCartItem(int id)
        {
            items.RemoveAll(s => s.productsInCart.ProductId == id);
        }

        // số lượng sp trong cart
        public int TotalAmountInCart()
        {
            return items.Sum(s => s.amountInCart);
        }

        // xóa giỏ hàng để thực hiện order
        public void ClearCart()
        {
            items.Clear();
        }

        //Thực hiện trong db

        public double TotalMoneyInCart()
        {
            var total = _context.Carts.Sum(t => (double?)t.Products_detail.Price * t.Amount) ?? 0d;
            return (double)total;
        }

        //list giỏ hàng
        public List<Cart> AllCartOfUser(string username)
        {
            //var cart = _context.Carts.Where(x => x.Username == username).ToList();
            var cart = (from i in _context.Carts
                        where i.Username == username
                        where i.Status == false
                        select i).ToList();
            return cart;
        }
        
        //list đơn hàng
        public List<Cart> AllOrderOfUser(string username)
        {
            //var cart = _context.Carts.Where(x => x.Username == username).ToList();
            var cart = (from i in _context.Carts
                        where i.Username == username
                        where i.Status == true
                        select i).ToList();
            return cart;
        }

        public Cart GetCart(string username, int id)
        {
            var cart = _context.Carts.Where(c => c.Username == username).SingleOrDefault(c => c.ProductId == id);
            return cart;
        }

        public int TotalAmountCartInDB(string username)
        {
            var amount = (from i in _context.Carts
                      where i.Username == username
                      select (int?)i.Amount).Sum() ?? 0;
            return amount;
        }
        public bool AddCartToDb(Cart cart, int amount = 1)
        {
            try
            {
                var item = _context.Carts.SingleOrDefault(p => p.ProductId == cart.ProductId);
                if (item == null)
                {
                    cart.Amount = amount;

                }
                else
                {
                    cart.Amount += amount;

                }
                _context.Carts.AddOrUpdate(cart);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateAmountInCart(Cart cart, int amount)
        {
            try
            {
                cart.Amount = amount;
                _context.Carts.AddOrUpdate(cart);
                _context.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCartFromDb(string Username, int id)
        {
            try
            {
                var cart = _context.Carts.Where(x => x.Username == Username).SingleOrDefault(x => x.ProductId == id);        
                _context.Carts.Remove(cart);
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