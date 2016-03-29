﻿using System;
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
    public class 客戶聯絡人Controller : Controller
    {
        private 客戶資料Entities1 db = new 客戶資料Entities1();
        客戶聯絡人Repository repo = RepositoryHelper.Get客戶聯絡人Repository();
        // GET: 客戶聯絡人
        public ActionResult 客戶聯絡人Index(string keyword = "")
        {
            var 客戶聯絡人 = repo.All().Include(客 => 客.客戶資料)
                .Where(user => (user.是否刪除 == false || user.是否刪除 == null)
                && (keyword == "" || keyword == null || user.姓名.Contains(keyword)));
            ViewBag.客戶名稱 = new SelectList(db.客戶資料.Where(cust => cust.是否刪除 == false), "Id", "客戶名稱");
            return View(客戶聯絡人.ToList());
        }

        // GET: 客戶聯絡人/Details/5
        public ActionResult 客戶聯絡人Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult 客戶聯絡人Create()
        {
            ViewBag.客戶Id = new SelectList(db.客戶資料.Where(cust => cust.是否刪除 == false), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult 客戶聯絡人Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                客戶聯絡人.是否刪除 = false;
                repo.Add(客戶聯絡人);
                repo.UnitOfWork.Commit();
                return RedirectToAction("客戶聯絡人Index");
            }

            ViewBag.客戶Id = new SelectList(db.客戶資料.Where(cust => cust.是否刪除 == false), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult 客戶聯絡人Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料.Where(cust => cust.是否刪除 == false), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult 客戶聯絡人Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                var dbCust = (客戶資料Entities1)repo.UnitOfWork.Context;
                客戶聯絡人.是否刪除 = false;
                dbCust.Entry(客戶聯絡人).State = EntityState.Modified;
                repo.UnitOfWork.Commit();
                return RedirectToAction("客戶聯絡人Index");
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料.Where(cust => cust.是否刪除 == false), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult 客戶聯絡人Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("客戶聯絡人Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult 客戶聯絡人DeleteConfirmed(int id)
        {
            var one = repo.Find(id);
            repo.Delete(one);
            repo.UnitOfWork.Commit();
            return RedirectToAction("客戶聯絡人Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }

        public FileResult ExportData()
        {
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("客戶名稱");
            row1.CreateCell(1).SetCellValue("職稱");
            row1.CreateCell(2).SetCellValue("姓名");
            row1.CreateCell(3).SetCellValue("Email");
            row1.CreateCell(4).SetCellValue("手機");
            row1.CreateCell(5).SetCellValue("電話");

            var i = 0;
            var data = repo.All().ToList();
            foreach (var item in data)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(item.客戶資料.客戶名稱);
                rowtemp.CreateCell(1).SetCellValue(item.職稱);
                rowtemp.CreateCell(2).SetCellValue(item.姓名);
                rowtemp.CreateCell(3).SetCellValue(item.Email);
                rowtemp.CreateCell(4).SetCellValue(item.手機);
                rowtemp.CreateCell(5).SetCellValue(item.電話);

                i++;
            }

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "客戶聯絡人.xls");
        }
    }
}
