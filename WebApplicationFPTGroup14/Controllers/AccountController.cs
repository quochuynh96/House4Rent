using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationFPTGroup14.Models;

namespace WebApplicationFPTGroup14.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SignUp()
        {
            var listCity = new List<City>();
            using (var db = new Group14Entities())
            {
                listCity = db.Cities.OrderBy(c => c.CityName).ToList();
            }
            return View(listCity);
        }

        [HttpPost]
        public ActionResult SignUp(FormCollection data)
        {
            var accountId = data["accountId"];
            var password = data["password"];
            var userName = data["userName"];
            DateTime birthDate = DateTime.Parse(data["birthDate"]);
            var address = data["address"];
            var phone = data["phone"];
            var email = data["email"];
            var city = data["city"];
            var district = data["district"];
            bool sex = true;
            if (data["sex"].Equals("1"))
            {
                sex = true;
            }
            else
            {
                sex = false;
            }
            bool role = true;
            if (data["role"].Equals("1"))
            {
                role = true;
            }
            else
            {
                role = false;
            }
            using (var db = new Group14Entities())
            {
                db.Accounts.Add(new Account() { AccountID = accountId, AccountStatusID = 2, Password = password, Role = role });
                db.Infoes.Add(new Info() { AccountID = accountId, Name = userName, Sex = sex, Birthday = birthDate, Phone = phone, Mail = email, CityID = Int32.Parse(city), TownshipID = Int32.Parse(district) });
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Login(string userInput, string passwordInput)
        {
            var StatusLogin = false;
            Account account = new Account();
            using (var db = new Group14Entities())
            {
                account = db.Accounts.SingleOrDefault(p => p.AccountID.ToLower().Equals(userInput.ToLower()) && p.Password.Equals(passwordInput));
                if (account != null && account.AccountStatusID == 1)
                {
                    var userName = db.Infoes.Single(p => p.AccountID == account.AccountID).Name;
                    Session["accountName"] = userName;
                    Session["account"] = account.AccountID;
                    if (account.Role == true)
                    {
                        Session["accountRole"] = "1";
                    }
                    else
                    {
                        Session["accountRole"] = "0";
                    }
                    StatusLogin = true;
                }
            }
            if (StatusLogin)
            {
                if (account.Role == true)
                {
                    return RedirectToAction("AllAccount", controllerName: "Admin");
                }
                else
                    return RedirectToAction("AllPosts", "Posts");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("account");
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult CheckAccount(string userInput, string passwordInput)
        {
            using (var db = new Group14Entities())
            {
                Account account = db.Accounts.SingleOrDefault(p => p.AccountID.Equals(userInput) && p.Password.Equals(passwordInput));
                if (account == null)
                {
                    return Json(new { success = true, result = false });
                }
            }
            return Json(new { success = true, result = true });
        }
        [HttpPost]
        public ActionResult CheckExist(string accountId)
        {
            using (var db = new Group14Entities())
            {
                var account = db.Accounts.FirstOrDefault(p => p.AccountID.Equals(accountId));
                if (account == null)
                {
                    return Json(new { success = true, result = false });
                }
                else
                {
                    return Json(new { success = true, result = true });
                }
            }
        }
        [HttpPost]
        public ActionResult Add(FormCollection data)
        {
            var account = new Account();
            account.AccountID = data["accountId"];
            account.Password = data["password"];
            account.Info = new Info();
            account.Info.Name = data["userName"];
            account.Info.Birthday = DateTime.Parse(data["birthDate"]);
            //account.HouseForRents
            return null;
        }

        [HttpGet]
        public ActionResult ChangePassWord()
        {
            if (Session["account"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassWord(FormCollection data)
        {
            var acountID = data["accountID"];
            var oldPassword = data["oldPassword"];
            var newpassword = data["newPassword"];
            using(var db = new Group14Entities())
            {
                var account = db.Accounts.Single(p => p.AccountID.Equals(acountID) && p.Password.Equals(oldPassword));
                account.Password = newpassword;
                db.SaveChanges();
            }
            Session.Remove("accountName");
            Session.Remove("account");
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public ActionResult CheckOldPassword(string accountID,string oldPassword)
        {
            using(var db = new Group14Entities())
            {
                Account account = db.Accounts.SingleOrDefault(p => p.AccountID.Equals(accountID) && p.Password.Equals(oldPassword));
                if (account != null)
                {
                    return Json(new { success = true, result = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, result = false }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        
    }
}