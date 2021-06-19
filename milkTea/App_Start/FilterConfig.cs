using milkTea.Models.ModelController;
using System.Web;
using System.Web.Mvc;

namespace milkTea
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());    
        }
    }
}
