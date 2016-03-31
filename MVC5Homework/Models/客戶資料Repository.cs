using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MVC5Homework.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public virtual IQueryable<客戶資料> All(string includeProperties = "")
        {
            return base.All().Include(includeProperties).Where(p => p.是否刪除 == false);
        }

        public 客戶資料 Find(int id)
        {
            return this.All().FirstOrDefault(i => i.Id == id);
        }

        public virtual void Delete(客戶資料 entity)
        {
            entity.是否刪除 = true;
        }
    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}