using MVC5Homework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MVC5Homework.Controllers
{
    public class BaseController : Controller
    {
        protected 客戶資料Entities1 db = new 客戶資料Entities1();
        protected 客戶聯絡人Repository contractRepo = RepositoryHelper.Get客戶聯絡人Repository();
        protected 客戶資料Repository custRepo = RepositoryHelper.Get客戶資料Repository();
        protected 客戶銀行資訊Repository bankRepo = RepositoryHelper.Get客戶銀行資訊Repository();
        protected int pageSize = 2;

        /// <summary>
        /// 雜湊密碼
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="password">密碼</param>
        /// <returns></returns>
        public string HashPassword(string password)
        {
            //account = account.ToLower();
            Byte[] data1ToHash = (new UnicodeEncoding()).GetBytes(password);
            byte[] hashvalue1 = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(data1ToHash);
            return Convert.ToBase64String(hashvalue1);
        }
    }
}