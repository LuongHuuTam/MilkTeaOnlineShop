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
            var res = (from i in _context.Categories
                       select i.CatId).ToList();

            return res.Max()+1;
        }

        public IEnumerable<Products_detail> getProByCat(int catId,int page, int pagesize)
        {
            if(catId==0)
            {
                var n = (from i in _context.Categories
                           select i.CatId).ToList();
                catId = n[0];
            }

            return _context.Products_detail.Where(x => x.CatId == catId).OrderBy(x => x.Name).ToPagedList(page, pagesize);
        }
        public int addCategory(string name)
        {
            Category res = new Category { CatId = getCatID(), CatName = name };
            _context.Categories.Add(res);
            _context.SaveChanges();
            return res.CatId;
        }
        public List<Products_detail> allProducts()
        {
            return _context.Products_detail.ToList();
        }

        //public List<> CategoryDetails(int id)
        //{

        //    var res = (from i in _context.Categories
        //               where Category.
        //               select i)
        //    return res;
        //}

        public bool DeleteProduct(int id)
        {
            try
            {
                Products_detail pro = _context.Products_detail.Where(x => x.ProductId == id).Single();
                foreach (var u in _context.DeliveryProducts)
                {
                    if (u.ProductId == id)
                    {
                        _context.DeliveryProducts.Remove(u);
                    }
                }
                foreach (var u in _context.Carts)
                {
                    if (u.ProductId == id)
                    {
                        _context.Carts.Remove(u);
                    }
                }
                _context.Products_detail.Remove(pro);
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