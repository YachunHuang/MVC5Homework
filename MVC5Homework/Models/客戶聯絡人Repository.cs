using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Collections;
using System.Linq.Expressions;

namespace MVC5Homework.Models
{
    public class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
    {
        private IQueryable<客戶聯絡人> _all;
        private string _includeProperties = "客戶資料";
        private IQueryable<客戶聯絡人> _includeResult;

        public override IQueryable<客戶聯絡人> All()
        {
            _all = base.All();

            return _all;
        }

        public IQueryable<客戶聯絡人> Include(string includeProperties)
        {
            _includeResult = All().Include(includeProperties);
            return _includeResult;
        }

        public IQueryable<客戶聯絡人> Where(string keyword, string title)
        {
            return All().Include(_includeProperties).Where(user => (user.是否刪除 == false || user.是否刪除 == null)
                && (keyword == "" || keyword == null || user.姓名.Contains(keyword))
                && (title == "" || title == null || user.職稱.Contains(title)) );
        }

        /// <summary>
        /// 依客戶ID取得客戶聯絡人
        /// </summary>
        /// <param name="id">客戶ID</param>
        /// <returns></returns>
        public IQueryable<客戶聯絡人> Where(int id)
        {
            return All().Include(_includeProperties).Where(user => (user.是否刪除 == false || user.是否刪除 == null) && user.客戶Id == id);
        }

        public 客戶聯絡人 Find(int id)
        {
            return All().FirstOrDefault(i => i.Id == id);
        }

        public virtual new void Delete(客戶聯絡人 entity)
        {
            entity.是否刪除 = true;
        }

        internal IEnumerable GetUserTitle()
        {
            return All().AsEnumerable().Select(u => u.職稱).Distinct().ToList();
        }
    }

    public interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
    {

    }
}