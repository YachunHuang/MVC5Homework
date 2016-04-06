using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace MVC5Homework.Models
{
    public static class RepositoryIQueryableExtensions
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> source, string path) where T : class
        {
            //這個檔案可以不需要作，因為預設就有支援Include了。
            // Ref: https://msdn.microsoft.com/en-us/library/system.data.entity.queryableextensions.include.aspx
            // See "Remarks"!

            if (source is ObjectQuery<T> || source is DbSet<T> ||
                source is DbQuery || source is DbSet)
            {
                return source.Include(path);
            }

            return source;
        }
    }
}