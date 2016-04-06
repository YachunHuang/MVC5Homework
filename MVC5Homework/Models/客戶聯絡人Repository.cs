using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace MVC5Homework.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public virtual IQueryable<客戶聯絡人> All(string includeProperties = "")
        {
            return base.All().Include(includeProperties).Where(p => p.是否刪除 == false);
        }

        public 客戶聯絡人 Find(int id)
        {
            return this.All().FirstOrDefault(i => i.Id == id);
        }

        public virtual void Delete(客戶聯絡人 entity)
        {
            entity.是否刪除 = true;
        }
    }

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}