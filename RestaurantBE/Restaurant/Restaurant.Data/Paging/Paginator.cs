using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Paging
{
    public class Paginator<T>
    {
        public IQueryable<T> Paginate(IQueryable<T> target, int page, int pageSize)
        {
            target = target.Skip((page - 1) * pageSize).Take(pageSize);
            return target;
        }

        internal List<Product> Paginate(List<Product> target, int page, int pageSize)
        {
            target = target.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return target;
        }
    }
}
