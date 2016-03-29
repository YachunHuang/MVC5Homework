using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5Homework.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(p => p.是否刪除 == false);
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