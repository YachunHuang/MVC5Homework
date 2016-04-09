using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace MVC5Homework.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
        private IQueryable<客戶銀行資訊> _all;
        private string _includeProperties = "客戶資料";
        private IQueryable<客戶銀行資訊> _includeResult;

        public virtual IQueryable<客戶銀行資訊> All(string includeProperties = "")
        {
            _all = base.All();

            return _all;
        }

        public IQueryable<客戶銀行資訊> Include(string includeProperties)
        {
            _includeResult = All().Include(includeProperties);
            return _includeResult;
        }

        public IQueryable<客戶銀行資訊> Where(string keyword)
        {
            return All().Include(_includeProperties).Where(客 => (客.是否刪除 == false || 客.是否刪除 == null) &&
            (keyword == "" || keyword == null ||
            客.帳戶名稱.Contains(keyword) ||
            客.銀行名稱.Contains(keyword) ||
            客.帳戶號碼.Contains(keyword) ||
            客.帳戶名稱.Contains(keyword) ||
            客.客戶資料.客戶名稱.Contains(keyword)));
        }

        public 客戶銀行資訊 Find(int id)
        {
            return All().FirstOrDefault(i => i.Id == id);
        }

        public virtual void Delete(客戶銀行資訊 entity)
        {
            entity.是否刪除 = true;
        }
    }

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}