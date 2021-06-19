using milkTea.Assets;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

        public IEnumerable<User_Accounts> listAllAccount(string search, int page, int pagesize)
        {
            var res = (from i in _context.User_Accounts
                       where i.Username.Contains(search) || i.LastName.Contains(search) || i.FirstName.Contains(search)
                       select i).ToList();
            return res.OrderBy(x => x.Type).ToPagedList(page, pagesize);
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
                if (user.Type == 1)
                {
                    _context.User_Accounts.Remove(user);
                    _context.SaveChanges();
                    return true;
                }
                foreach(var u in _context.Orders_Detail)
                {
                    if (u.Customer == Username)
                    {
                        foreach(var x in _context.Orders)
                        {
                            if (x.Orders_Detail == u.OrderDetailId)
                                _context.Orders.Remove(x);
                        }
                        _context.Orders_Detail.Remove(u);
                    }
                }
                foreach (var u in _context.Carts)
                {
                    if (u.Username == Username)
                    {
                        _context.Carts.Remove(u);
                    }
                }
                foreach (var u in _context.Products_Detail)
                {
                    if (u.Seller == Username)
                    {
                        foreach(var n in _context.Carts)
                        {
                            if (n.ProductId==u.ProductId)
                            {
                                _context.Carts.Remove(n);
                            }
                        }
                        foreach (var m in _context.Orders)
                        {
                            if (m.ProductId == u.ProductId)
                            {
                                _context.Orders.Remove(m);
                            }
                        }
                        _context.Products_Detail.Remove(u);
                    }
                }
                _context.User_Accounts.Remove(user);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public User_Accounts getAccount(string username)
        {
            return _context.User_Accounts.Where(x => x.Username == username).SingleOrDefault();
        }

        public bool updateAccount(User_Accounts user)
        {
            try
            {
                _context.User_Accounts.AddOrUpdate(user);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool checkEmail(string email)
        {
            return _context.User_Accounts.Any(x => x.Email == email);
        }
        public void resetPass(string email)
        {
            var user = _context.User_Accounts.Where(x => x.Email == email).FirstOrDefault();
            if (user!=null)
            {
                user.Password= Encryptor.MD5Hash("Password@123");
                _context.SaveChanges();
            }
        }
    }
}