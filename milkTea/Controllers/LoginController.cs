using milkTea.Assets;
using milkTea.Models;
using milkTea.Models.ModelController;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace milkTea.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(User_Accounts us)
        {
            User_Accounts res = new AccountModel().login(us.Username, Encryptor.MD5Hash(us.Password));
            if (res != null)
            {
                Session["user"] = res;
                if (res.Type == 1)
                    return RedirectToAction("index", "admin");
                CartModel cart = Session["Cart"] as CartModel;
                //lưu session cart vào db
                if (cart != null)
                {
                    var cartInDb = new Cart();
                    foreach (var item in cart.Items)
                    {
                        cartInDb.Username = res.Username;
                        cartInDb.ProductId = item.productsInCart.ProductId;
                        if (new CartModel().AddCartSessionToDb(cartInDb, item.amountInCart, res.Username))
                        { }
                    }
                    Session["Cart"] = null;
                }
                return RedirectToAction("index", "home");
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            }
            return View(us);
        }
        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("index", "home");
        }
        [HttpGet]
        public ActionResult registerAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult registerAccount(User_Accounts model, string rePass)
        {
            if (model.Password != rePass || model.Password == null)
            {
                ModelState.AddModelError("", "Hai mật khẩu không trùng khớp");
                return View(model);
            }
            string require = checkPassword(model.Password);
            if(!string.IsNullOrEmpty(require))
            {
                ModelState.AddModelError("", require);
                return View(model);
            }

            model.Password = Encryptor.MD5Hash(model.Password);
            model.Type = 3;
            model.Avatar_url = "/Photo/images/default.png";
            if (new AccountModel().CreateAccount(model))
            {
                return View("index");
            }
            ModelState.AddModelError("", "Lỗi... Hãy thử lại");
            return View(model);
        }


        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Please enter your email");
                return View();
            }

            if (!new AccountModel().checkEmail(email))
            {
                ModelState.AddModelError("", "Email is incorrect");
                return View();
            }
            string code = GetCode();
            ResetPass reset = new ResetPass
            {
                Email = email,
                Code = code
            };
            Session["reset"] = reset;
            string content = "Code Reset Password: " + code;
            SendMail(email, "RESET PASSWORD", content);
            return RedirectToAction("Confirm");
        }

        [HttpGet]
        public ActionResult Confirm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Confirm(string code)
        {
            ResetPass reset = Session["reset"] as ResetPass;
            if (reset.Code==code)
            {
                new AccountModel().resetPass(reset.Email);
                return RedirectToAction("Done");
            }
            ModelState.AddModelError("", "Code is incorrect");
            return View();
        }

        public ActionResult Done()
        {
            Session["reset"] = null;
            return View();
        }


        private string GetCode()
        {
            StringBuilder sb = new StringBuilder();
            char c;
            Random rand = new Random();
            for (int i = 0; i < 6; i++)
            {
                c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
                sb.Append(c);
            }            
            return sb.ToString();
        }

        public void SendMail(string toEmailAddress,string subject, string content)
        {
            var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
            var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
            var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
            var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();

            bool enableSSL = bool.Parse(ConfigurationManager.AppSettings["EnableSSL"].ToString());

            string body = content;
            MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmailAddress));
            message.Subject = subject;
            message.Body = body;

            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.Host = smtpHost;
            client.EnableSsl = enableSSL;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            client.Send(message);
        }

        private string checkPassword(string password)
        {
            string res = "";
            if (password.Length <= 6)
                res += "Password at least 6 characters;";
            if (!hasLowerlChar(password))
                res += "Password must have lowercase characters;";
            if (!hasUpperChar(password))
                res += "Password must have uppercase characters;";
            if (!hasSpecialChar(password))
                res += "Password must have special characters;";
            if (!hasNumber(password))
                res += "Password must have number characters;";
            return res;
        }


        private bool hasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
        private bool hasUpperChar(string input)
        {
            string specialChar = @"QWERTYUIOPLKJHGFDSAZXCVBNM";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
        private bool hasLowerlChar(string input)
        {
            string specialChar = @"qwertyuioplkjhgfdsazxcvbnm";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
        private bool hasNumber(string input)
        {
            string specialChar = @"1234567890";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
    }
}