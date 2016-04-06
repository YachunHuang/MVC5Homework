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
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

namespace MVC5Homework.Controllers
{
    public class 客戶銀行資訊Controller : BaseController
    {
        // GET: 客戶銀行資訊
        public ActionResult 客戶銀行資訊Index(string keyword)
        { 
            var 客戶銀行資訊 = bankRepo.All(includeProperties: "客戶資料")
                .Where(客 => (客.是否刪除 == false || 客.是否刪除 == null) &&
            (keyword == "" || keyword == null ||
            客.帳戶名稱.Contains(keyword) ||
            客.銀行名稱.Contains(keyword) ||
            客.帳戶號碼.Contains(keyword)  ||
            客.帳戶名稱.Contains(keyword) ||
            客.客戶資料.客戶名稱.Contains(keyword)));
            return View(客戶銀行資訊.ToList());
        }

        // GET: 客戶銀行資訊/Details/5
        public ActionResult 客戶銀行資訊Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = bankRepo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Create
        public ActionResult 客戶銀行資訊Create()
        {
            ViewBag.客戶Id = new SelectList(custRepo.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶銀行資訊/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult 客戶銀行資訊Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                客戶銀行資訊.是否刪除 = false;
                bankRepo.Add(客戶銀行資訊);
                bankRepo.UnitOfWork.Commit();
                return RedirectToAction("客戶銀行資訊Index");
            }

            ViewBag.客戶Id = new SelectList(custRepo.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Edit/5
        public ActionResult 客戶銀行資訊Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = bankRepo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(custRepo.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult 客戶銀行資訊Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                var dbCust = (客戶資料Entities1)bankRepo.UnitOfWork.Context;
                客戶銀行資訊.是否刪除 = false;
                dbCust.Entry(客戶銀行資訊).State = EntityState.Modified;
                bankRepo.UnitOfWork.Commit();
                return RedirectToAction("客戶銀行資訊Index");
            }
            ViewBag.客戶Id = new SelectList(custRepo.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Delete/5
        public ActionResult 客戶銀行資訊Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = bankRepo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Delete/5
        [HttpPost, ActionName("客戶銀行資訊Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult 客戶銀行資訊DeleteConfirmed(int id)
        {
            var one = bankRepo.Find(id);
            bankRepo.Delete(one);
            bankRepo.UnitOfWork.Commit();
            return RedirectToAction("客戶銀行資訊Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bankRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }

        public FileResult ExportData()
        {
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("客戶名稱");
            row1.CreateCell(1).SetCellValue("銀行名稱");
            row1.CreateCell(2).SetCellValue("銀行代碼");
            row1.CreateCell(3).SetCellValue("分行代碼");
            row1.CreateCell(4).SetCellValue("帳戶名稱");
            row1.CreateCell(5).SetCellValue("帳戶號碼");

            var i = 0;
            var data = bankRepo.All().ToList();
            foreach (var item in data)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(item.客戶資料.客戶名稱);
                rowtemp.CreateCell(1).SetCellValue(item.銀行名稱);
                rowtemp.CreateCell(2).SetCellValue(item.銀行代碼);
                rowtemp.CreateCell(3).SetCellValue(Convert.ToString(item.分行代碼));
                rowtemp.CreateCell(4).SetCellValue(item.帳戶名稱);
                rowtemp.CreateCell(5).SetCellValue(Convert.ToString(item.帳戶號碼));

                i++;
            }

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "客戶銀行資訊.xls");
        }
    }
}