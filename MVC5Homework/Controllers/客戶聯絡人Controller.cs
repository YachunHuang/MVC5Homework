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
using PagedList;
using System.Linq.Dynamic;

namespace MVC5Homework.Controllers
{
    public class 客戶聯絡人Controller : BaseController
    {
        // GET: 客戶聯絡人
        public ActionResult 客戶聯絡人Index(string keyword, string 職稱, string sortOrder, int page = 1)
        {
            sortOrder = string.IsNullOrEmpty(sortOrder) ? "姓名 desc" : sortOrder;

            ViewBag.NameSortParm = sortOrder == "姓名" ? "姓名 desc" : "姓名";
            ViewBag.職稱 = new SelectList(contractRepo.GetUserTitle(), "", "");
            ViewBag.客戶名稱 = new SelectList(custRepo.Where(cust => cust.是否刪除 == false), "Id", "客戶名稱");

            return View(contractRepo.Where(keyword, 職稱).OrderBy(sortOrder).ToPagedList(page, pageSize));
        }

        // GET: 客戶聯絡人/Details/5
        public ActionResult 客戶聯絡人Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = contractRepo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        [Authorize(Roles = "board_admin")]
        // GET: 客戶聯絡人/Create
        public ActionResult 客戶聯絡人Create()
        {
            ViewBag.客戶Id = new SelectList(custRepo.All(), "Id", "客戶名稱");
            return View();
        }

        [Authorize(Roles = "board_admin")]
        // POST: 客戶聯絡人/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [HandleError(ExceptionType = typeof(InvalidOperationException), View = "Error2")]
        [ValidateAntiForgeryToken]
        public ActionResult 客戶聯絡人Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                if(客戶聯絡人.姓名 == "123")
                {

                    throw new ArgumentException("ArgumentException客戶聯絡人Create發生錯誤");
                }
                throw new InvalidOperationException("InvalidOperationException客戶聯絡人Create發生錯誤");

                客戶聯絡人.是否刪除 = false;
                contractRepo.Add(客戶聯絡人);
                contractRepo.UnitOfWork.Commit();
                return RedirectToAction("客戶聯絡人Index");
            }

            ViewBag.客戶Id = new SelectList(custRepo.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        [Authorize(Roles = "board_admin")]
        // GET: 客戶聯絡人/Edit/5
        public ActionResult 客戶聯絡人Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = contractRepo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(custRepo.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        [Authorize(Roles = "board_admin")]
        // POST: 客戶聯絡人/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult 客戶聯絡人Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                var dbCust = (客戶資料Entities1)contractRepo.UnitOfWork.Context;
                客戶聯絡人.是否刪除 = false;
                dbCust.Entry(客戶聯絡人).State = EntityState.Modified;
                contractRepo.UnitOfWork.Commit();
                return RedirectToAction("客戶聯絡人Index");
            }
            ViewBag.客戶Id = new SelectList(custRepo.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        [Authorize(Roles = "board_admin")]
        // GET: 客戶聯絡人/Delete/5
        public ActionResult 客戶聯絡人Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = contractRepo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        [Authorize(Roles = "board_admin")]
        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("客戶聯絡人Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult 客戶聯絡人DeleteConfirmed(int id)
        {
            var one = contractRepo.Find(id);
            contractRepo.Delete(one);
            contractRepo.UnitOfWork.Commit();
            return RedirectToAction("客戶聯絡人Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                contractRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }

        public FileResult ExportData(string keyword, string 職稱)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            var data = contractRepo.Where(keyword, 職稱).ToList();

            row1.CreateCell(0).SetCellValue("客戶名稱");
            row1.CreateCell(1).SetCellValue("職稱");
            row1.CreateCell(2).SetCellValue("姓名");
            row1.CreateCell(3).SetCellValue("Email");
            row1.CreateCell(4).SetCellValue("手機");
            row1.CreateCell(5).SetCellValue("電話");

            var i = 0;
            
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

            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "客戶聯絡人.xls");
        }
    }
}