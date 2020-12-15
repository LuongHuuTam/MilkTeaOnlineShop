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

        public IEnumerable<Products_detail> getProByCat(int catId, int page, int pagesize)
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
        public IEnumerable<Products_detail> allProducts(int page, int pagesize)
        {
            return _context.Products_detail.OrderBy(x => x.Name).ToPagedList(page, pagesize);
        }

        public string getNameByCatID(int id)
        {
            var res = _context.Categories.Where(x => x.CatId == id).SingleOrDefault();
            return res.CatName;
        }

        public IEnumerable<Products_detail> searchProducts(string serch, int page, int pagesize)
        {
            var res = (from i in _context.Products_detail
                       where i.Name.Contains(serch)
                       select i).OrderBy(p => p.Name).ToPagedList(page, pagesize);
            return res;
        }
    }
}