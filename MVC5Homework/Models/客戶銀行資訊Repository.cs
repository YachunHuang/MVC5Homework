using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace MVC5Homework.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
        public virtual IQueryable<客戶銀行資訊> All(string includeProperties = "")
        {
            //string includeProperties = ""
            return base.All().Include(includeProperties).Where(p => p.是否刪除 == false);
        }

        public 客戶銀行資訊 Find(int id)
        {
            return this.All().FirstOrDefault(i => i.Id == id);
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