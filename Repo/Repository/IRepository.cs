using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repo.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>> expression, List<string> references);
        IQueryable<T> GetAllQuerable(Expression<Func<T, bool>> expression, List<string> references);
        IEnumerable<T> GetAllPaginated(Expression<Func<T, bool>> expression, int page, int pageSize, List<string> references, bool ascending = true);
        T Get(Expression<Func<T, bool>> expression, List<string> references);
        T Get(long id);
        int Count(Expression<Func<T, bool>> expression);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}
