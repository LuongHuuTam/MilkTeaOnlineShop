using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace milkTea.Models.ModelController
{
    public class AccountModel
    {
        private MilkTeaModel _context = null;
        public AccountModel()
        {
            _context = new MilkTeaModel();
        }

        public User_Accounts login(string username,string pass)
        {
            int res = _context.User_Accounts.Count(x => x.Username == username && x.Password == pass);
            if (res == 1)
                return _context.User_Accounts.Where(x => x.Username == username).Single();
            return null;
        }
    }
}