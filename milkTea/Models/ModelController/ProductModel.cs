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
    }
}