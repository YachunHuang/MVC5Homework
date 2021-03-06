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
using PagedList;
using System.Linq.Dynamic;

namespace MVC5Homework.Controllers
{
    [計算Action的執行時間]
    public class 客戶資料Controller : BaseController
    {
        // GET: 客戶資料
        public ActionResult 客戶資料Index(string keyword, string sortOrder, int? 客戶分類Id = 0, int page = 1)
        {
            ViewBag.客戶分類Id = new SelectList(db.客戶分類, "Id", "分類", new { Id = "" });
            ViewBag.分類 = new SelectList(db.客戶分類, "Id", "分類");

            var custom = custRepo.Sort(keyword, sortOrder, 客戶分類Id);
            page = custom.ToPagedList(page, pageSize).PageCount < page ? 1 : page;

            return View(custom.ToPagedList(page, pageSize));
        }

        /// <summary>
        /// 客戶聯絡人Partial給資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult 客戶聯絡人Partial(int id)
        {
            return PartialView(contractRepo.Where(id).ToList());
        }

        // GET: 客戶資料/Details/5
        public ActionResult 客戶資料Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = custRepo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }

            return View(客戶資料);
        }
        [HttpPost]
        public ActionResult 客戶聯絡人Partial(IList<BatchUpdateCust> data)
        {
            if (ModelState.IsValid && data != null)
            {
                var custId = 0;
                foreach (var item in data)
                {
                    var prod = db.客戶聯絡人.Find(item.Id);
                    custId = prod.客戶資料.Id;
                    prod.職稱 = item.職稱;
                    prod.手機 = item.手機;
                    prod.電話 = item.電話;
                }
                db.SaveChanges();

                var 客戶聯絡人 = db.客戶聯絡人.Where(u => u.是否刪除 == false && u.客戶Id == custId);
                return PartialView(客戶聯絡人.ToList());
            }

            return View();
        }


        // GET: 客戶資料/Create
        public ActionResult 客戶資料Create()
        {
            ViewBag.客戶分類Id = new SelectList(db.客戶分類, "Id", "分類");
            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult 客戶資料Create([Bind(Include = "Id,帳號,密碼,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類Id")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                客戶資料.是否刪除 = false;
                客戶資料.密碼 = HashPassword(客戶資料.密碼);
                custRepo.Add(客戶資料);
                custRepo.UnitOfWork.Commit();
                return RedirectToAction("客戶資料Index");
            }

            ViewBag.客戶分類Id = new SelectList(db.客戶分類, "Id", "分類", 客戶資料.客戶分類Id);
            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult 客戶資料Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = custRepo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            TempData["PassWord"] = 客戶資料.密碼;
            ViewBag.客戶分類Id = new SelectList(db.客戶分類, "Id", "分類", 客戶資料.客戶分類Id);
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult 客戶資料Edit([Bind(Include = "Id,帳號,密碼,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類Id")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                var dbCust = (客戶資料Entities1)custRepo.UnitOfWork.Context;
                客戶資料.是否刪除 = false;

                //密碼有修改的話則需要重新編碼
                if (客戶資料.密碼 != Convert.ToString(TempData["PassWord"]))
                {
                    客戶資料.密碼 = HashPassword(客戶資料.密碼);
                }

                dbCust.Entry(客戶資料).State = EntityState.Modified;
                custRepo.UnitOfWork.Commit();
                return RedirectToAction("客戶資料Index");
            }
            ViewBag.客戶分類Id = new SelectList(db.客戶分類, "Id", "分類", 客戶資料.客戶分類Id);
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult 客戶資料Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = custRepo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("客戶資料Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult 客戶資料DeleteConfirmed(int id)
        {
            var one = custRepo.Find(id);
            custRepo.Delete(one);
            custRepo.UnitOfWork.Commit();
            return RedirectToAction("客戶資料Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                custRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
 
        //[HttpPost]
        public FileResult ExportData(string keyword, int 客戶分類Id)
        {
            var custtype = 客戶分類Id;
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("分類");
            row1.CreateCell(1).SetCellValue("客戶名稱");
            row1.CreateCell(2).SetCellValue("統一編號");
            row1.CreateCell(3).SetCellValue("電話");
            row1.CreateCell(4).SetCellValue("傳真");
            row1.CreateCell(5).SetCellValue("地址");
            row1.CreateCell(6).SetCellValue("Email");

            var i = 0;
            var data = custRepo.Where(keyword, custtype).ToList();
            foreach (var item in data)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(item.客戶分類.分類);
                rowtemp.CreateCell(1).SetCellValue(item.客戶名稱);
                rowtemp.CreateCell(2).SetCellValue(item.統一編號);
                rowtemp.CreateCell(3).SetCellValue(item.電話);
                rowtemp.CreateCell(4).SetCellValue(item.傳真);
                rowtemp.CreateCell(5).SetCellValue(item.地址);
                rowtemp.CreateCell(6).SetCellValue(item.Email);

                i++;
            }

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "客戶資料.xls");
        }
    }
}
