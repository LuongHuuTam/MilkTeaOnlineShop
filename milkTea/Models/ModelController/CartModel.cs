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
        public Products_Detail productsInCart { get; set; }
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

        public void Add(Products_Detail products, int amount = 1)
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

        public Products_Detail GetProducts(int id)
        {
            var pro = _context.Products_Detail.SingleOrDefault(p => p.ProductId == id);
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

        //xóa giỏ hàng trong session
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
        public float TotalMoneyInCart(string username)
        {
            var total = _context.Carts.Where(u => u.Username == username).Sum(t => (double?)t.Products_Detail.Price * t.Amount) ?? 0f;
            return (float)total;
        }

        //list giỏ hàng
        public List<Cart> AllCartOfUser(string username)
        {
            var cart = (from i in _context.Carts
                        where i.Username == username
                        select i).ToList();
            return cart;
        }

        public Cart GetCart(string username, int id)
        {
            var cart = (from i in _context.Carts
                        where i.Username == username
                        select i).SingleOrDefault(c => c.ProductId == id);
            return cart;
        }

        public int TotalAmountCartInDB(string username)
        {
            var amount = (from i in _context.Carts
                      where i.Username == username
                      select (int?)i.Amount).Sum() ?? 0;
            return amount;
        }

        public bool AddCartToDb(Cart cart, int amount)
        {
            try
            {
                var item = _context.Carts.SingleOrDefault(p => p.ProductId == cart.ProductId);
                if (item == null)
                {
                    cart.Amount = 1;
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

        public bool DeleteCartFromDb(string username, int id)
        {
            try
            {
                var cart = (from i in _context.Carts
                            where i.Username == username
                            select i).SingleOrDefault(c => c.ProductId == id);
                _context.Carts.Remove(cart);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddCartSessionToDb(Cart cart, int amount, string username)
        {
            try
            {
                var product = _context.Products_Detail.SingleOrDefault(p => p.ProductId == cart.ProductId);
                if (product.Seller != username)
                {
                    var item = _context.Carts.SingleOrDefault(p => p.ProductId == cart.ProductId);
                    if (item == null)
                    {
                        cart.Amount = amount;
                        _context.Carts.Add(cart);
                    }
                    else
                    {
                        item.Amount += amount;
                        _context.Carts.AddOrUpdate(item);
                    }
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}