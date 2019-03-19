using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCMusicStoreCodeFirst.Models;

namespace MVCMusicStoreCodeFirst.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            using (AccountDBContext db = new AccountDBContext())
            {
                return View(db.userAccount.ToList());
            }
        }

        //Signup
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserAccount account)
        {
            if (ModelState.IsValid)
            {
                using (AccountDBContext db = new AccountDBContext())
                {
                    db.userAccount.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.firstName + " " + account.lastName + " successfully registered";
            }
            return View();
        }

        //Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
            using (AccountDBContext db = new AccountDBContext())
            {
                try
                {
                    var usr = db.userAccount.Single(u => u.Username == user.Username && u.Password == user.Password);
                    if (usr != null)
                    {
                        Session["UserID"] = usr.UserID.ToString();
                        Session["Username"] = usr.Username.ToString();
                        return RedirectToAction("LoggedIn");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Username or Password are incorrect");
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("Login");
                }
            }

            return View();
        }

        //Loggedin
        public ActionResult LoggedIn()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}