using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.Entity;
using System.Linq.Dynamic;

namespace MVC5Homework.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        private IQueryable<客戶資料> _all;
        private string _includeProperties = "客戶分類";
        private IQueryable<客戶資料> _includeResult;

        public virtual new IQueryable<客戶資料> All()
        {
            _all = base.All();

            return _all;
        }

        public IQueryable<客戶資料> Include(string includeProperties)
        {
            _includeResult = All().Include(includeProperties);
            return _includeResult;
        }

        public IQueryable<客戶資料> Where(string keyword, int? custTypeId)
        {
            return All().Include(_includeProperties).Where(客 => 客.是否刪除 == false &&
             (keyword == "" || keyword == null || 客.客戶名稱.Contains(keyword)) &&
             (客.客戶分類Id == custTypeId || custTypeId == 0));
        }

        public IEnumerable<客戶資料> Sort(string keyword, string sortOrder, int? custTypeId)
        {
            var customers = Where(keyword, custTypeId).OrderBy(s => s.客戶名稱).ToList();
            sortOrder = string.IsNullOrEmpty(sortOrder) ? "客戶名稱 desc" : sortOrder;
            if (sortOrder.Contains("分類 desc"))
                customers = Where(keyword, custTypeId).OrderByDescending(s => s.客戶分類.分類).ToList();
            else {
                if (sortOrder.Contains("分類"))
                    customers = Where(keyword, custTypeId).OrderBy(s => s.客戶分類.分類).ToList();
                else
                    customers = Where(keyword, custTypeId).OrderBy(sortOrder).ToList();
            }

            return customers;
        }

        /// <summary>
        /// 判斷登入帳號
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        internal 客戶資料 GetCustData(string account)
        {
            return All().Include(_includeProperties).Where(客 => 客.是否刪除 == false && 客.帳號 == account).FirstOrDefault();
        }

        public 客戶資料 Find(int id)
        {
            return All().FirstOrDefault(i => i.Id == id);
        }

        public virtual new void Delete(客戶資料 entity)
        {
            entity.是否刪除 = true;
        }
    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}