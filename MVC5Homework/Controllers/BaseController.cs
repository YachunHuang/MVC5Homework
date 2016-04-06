using MVC5Homework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
       
    }
}