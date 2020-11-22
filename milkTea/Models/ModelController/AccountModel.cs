using PagedList;
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

        public User_Accounts login(string username, string pass)
        {
            int res = _context.User_Accounts.Count(x => x.Username == username && x.Password == pass);
            if (res == 1)
                return _context.User_Accounts.Where(x => x.Username == username).Single();
            return null;
        }

        public IEnumerable<User_Accounts> listAllAccount(int page, int pagesize)
        {

            return _context.User_Accounts.OrderBy(x => x.Type).ToPagedList(page, pagesize);
        }

        public bool CreateAccount(User_Accounts user)
        {
            try
            {
                _context.User_Accounts.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool DeleteAccount(string Username)
        {
            try
            {
                User_Accounts user = _context.User_Accounts.Where(x => x.Username == Username).Single();
                //if (user.Type == 1)
                //{
                //    _context.User_Accounts.Remove(user);
                //    _context.SaveChanges();
                //    return true;
                //}
                _context.User_Accounts.Remove(user);
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