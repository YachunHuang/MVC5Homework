using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Homework.Models;
using System.IO;

namespace MVC5Homework.Controllers
{
    public class CustomController : BaseController
    {
        // GET: Custom
        public ActionResult Index()
        {
            return View(custRepo.All().ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ReadCustomView()
        {
            //var data = db.CustomView.AsQueryable();
            return View();
        }

        public JsonResult GetTotalCust()
        {
            var data = db.CustomView.AsQueryable();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public FileResult ExportData()
        {
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("客戶名稱");
            row1.CreateCell(1).SetCellValue("聯絡人數量");
            row1.CreateCell(2).SetCellValue("銀行帳戶數量");

            var i = 0;
            foreach(var item in db.CustomView.AsEnumerable())
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(item.客戶名稱);
                rowtemp.CreateCell(1).SetCellValue(item.聯絡人數量.ToString());
                rowtemp.CreateCell(2).SetCellValue(item.銀行帳戶數量.ToString());

                i++;
            }

            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "ReadCustomView.xls");
        }
    }
}