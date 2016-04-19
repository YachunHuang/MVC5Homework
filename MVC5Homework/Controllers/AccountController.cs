using Microsoft.AspNet.Identity;
using MVC5Homework.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace MVC5Homework.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        string userData = "";

        [AllowAnonymous]
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel data)
        {

            // 登入時清空所有 Session 資料
            Session.RemoveAll();

            if (CheckLogin(data))
            {
                FormsAuthentication.RedirectFromLoginPage(data.Account, false);

                // 將管理者登入的 Cookie 設定成 Session Cookie
                bool isPersistent = false;

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                  data.Account,
                  DateTime.Now,
                  DateTime.Now.AddMinutes(30),
                  isPersistent,
                  userData,
                  FormsAuthentication.FormsCookiePath);

                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                return RedirectToAction("ReadCustomView", "Custom");
            }
            else
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "LoginFail", "alert('登入失敗，請重新登入!');", true);
            }
            return View();
        }

        private bool CheckLogin(LoginViewModel data)
        {
            bool isPass = true;
            客戶資料 客戶資料 = custRepo.GetCustData(data.Account);
            if (客戶資料 != null)
            {
                if (客戶資料.密碼 != HashPassword(data.Password))
                {
                    isPass = false;
                }
                else
                {
                    if (data.Account == "admin")
                        userData = "gold_member,board_admin";
                    else
                        userData = "gold_member";
                }
            }

            return isPass;
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel data)
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EditProfile(string account)
        {
            account = string.IsNullOrEmpty(account) ? User.Identity.GetUserName() : account;

            客戶資料 客戶資料 = db.客戶資料.Include(客 => 客.客戶分類).
                Where(客 => 客.是否刪除 == false && 客.帳號 == account).FirstOrDefault();

            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            TempData["PassWord"] = 客戶資料.密碼;
            return View(客戶資料);
        }

        [HttpPost]
        public ActionResult EditProfile(string account, string 密碼)
        {
            var dbCust = custRepo.GetCustData(User.Identity.GetUserName());
            //密碼有修改的話則需要重新編碼
            if (dbCust.密碼 != 密碼)
            {
                dbCust.密碼 = HashPassword(密碼);
            }

            if (TryUpdateModel(dbCust, new string[] {"電話", "傳真", "地址", "Email" }))
            {
                custRepo.UnitOfWork.Commit();
                return RedirectToAction("ReadCustomView", "Custom");
            }
            return View();
        }
    }
}