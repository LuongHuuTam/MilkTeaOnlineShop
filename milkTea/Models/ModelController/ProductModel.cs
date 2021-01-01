using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace milkTea.Models.ModelController
{
    public class ProductModel
    {
        private MilkTeaModel _context = null;
        public ProductModel()
        {
            _context = new MilkTeaModel();
        }

        public List<Category> allCategory()
        {
            var res = (from i in _context.Categories
                       select i).ToList();
            return res;
        }
        public int getCatID()
        {
            var res = (from n in _context.Categories
                       select n.CatId).ToList();
            if (res.Count <= 0 || res[0] != 1)
                return 1;
            int i;
            for (i = 0; i < res.Count - 1; i++)
            {
                if (res[i + 1] - res[i] != 1)
                    break;
            }

            return res[i] + 1;
        }

        public IEnumerable<Products_Detail> getProByCat(int catId, int page, int pagesize)
        {
            if (catId == 0)
            {
                var n = (from i in _context.Categories
                         select i.CatId).ToList();
                try
                {
                    catId = n[0];
                }
                catch
                {
                    catId = 0;
                }
            }

            return _context.Products_Detail.Where(x => x.CatId == catId).OrderBy(x => x.Name).ToPagedList(page, pagesize);
        }
        public int addCategory(string name)
        {
            Category res = new Category { CatId = getCatID(), CatName = name };
            _context.Categories.Add(res);
            _context.SaveChanges();
            return res.CatId;
        }
        public IEnumerable<Products_Detail> allProducts(int page, int pagesize)
        {
            return _context.Products_Detail.OrderBy(x => x.Name).ToPagedList(page, pagesize);
        }

        public bool DeleteCategory(int id)
        {
            try
            {
                Category cat = _context.Categories.Where(x => x.CatId == id).Single();
                var temp = _context.Products_Detail.Where(x => x.CatId == id).ToList();
                foreach (var i in temp)
                {
                    DeleteProduct(i.ProductId);
                }
                _context.Categories.Remove(cat);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteProduct(int id)
        {
            try
            {
                Products_Detail pro = _context.Products_Detail.Where(x => x.ProductId == id).Single();
                foreach (var u in _context.Orders)
                {
                    if (u.ProductId == id)
                    {
                        _context.Orders.Remove(u);
                    }
                }
                foreach (var u in _context.Carts)
                {
                    if (u.ProductId == id)
                    {
                        _context.Carts.Remove(u);
                    }
                }
                _context.Products_Detail.Remove(pro);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string getNameByCatID(int id)
        {
            var res = _context.Categories.Where(x => x.CatId == id).SingleOrDefault();
            return res.CatName;
        }

        public IEnumerable<Products_Detail> searchProducts(string serch, int page, int pagesize)
        {
            var res = (from i in _context.Products_Detail
                       where i.Name.Contains(serch)
                       select i).OrderBy(p => p.Name).ToPagedList(page, pagesize);
            return res;
        }

        public IEnumerable<Products_Detail> allProductsOfAnotherUser(string username, int page, int pagesize)
        {
            //return _context.Products_Detail.OrderBy(x => x.Name).ToPagedList(page, pagesize);
            var product = (from i in _context.Products_Detail
                           where i.Seller != username
                           orderby i.Name
                           select i).ToPagedList(page, pagesize);
            return product;
        }
        //---------
        
         public List<Size> allSize()
        {
            return _context.Sizes.ToList();
        }
        public bool AddProduct(Products_Detail product)
        {
            try
            {
                _context.Products_Detail.Add(product);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Products_Detail GetProductById(int? Id)
        {
            return _context.Products_Detail.Find(Id);
        }
        public List<Products_Detail> GetProductBySeller(User_Accounts seller)
        {

            return _context.Products_Detail.Where(x => x.Seller == seller.Username).ToList();
        }
        public bool UpdateProduct(Products_Detail pro)
        {
            try
            {
                var product =_context.Products_Detail.Find(pro.ProductId);
                product.Seller = pro.Seller;
                product.Name = pro.Name;
                product.Imgage_url = pro.Imgage_url;
                product.Desciption = pro.Desciption;
                product.CatId = pro.CatId;
                product.Size = pro.Size;
                product.Price = pro.Price;
                product.Quantity = pro.Quantity;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Products_Detail GetProductByOrder(Order order)
        {
            return _context.Products_Detail.Find(order.ProductId);
        }
    }
}
