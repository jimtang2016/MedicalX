using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Impls 
{
    public abstract class Repository<T,TContext> :RepositoryBase<TContext> ,IRepository<T>
        where T : class, new()
        where TContext : DbContext, IDisposable 
    {
        protected readonly IDbSet<T> Dbset;

        protected Repository(IDataContextFactory<TContext> databaseFactory) :
            base(databaseFactory)
        {
            Dbset = DataContext.Set<T>();
        }


        #region IRepository<T> Members

        public void Add(IEnumerable<T> entities)
        {
            foreach (var item in entities)
            {
                Add(item);
            }
        }

        public void Add(T entity)
        {
            Dbset.Add(entity);
        }

        public void Delete(System.Linq.Expressions.Expression<Func<T, bool>> where)
        {

            var objects = Dbset.Where(@where).AsEnumerable();
            foreach (var obj in objects)
                Dbset.Remove(obj);
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            Dbset.Remove(Dbset.Find(id));
           
        }

        public void Delete(T entity)
        {
            Dbset.Remove(entity);
        }

        public List<T> ExecuteStoredProcedure<T>(string procedureName, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public T Get(System.Linq.Expressions.Expression<Func<T, bool>> where)
        {
            var objects = Dbset.Where(@where).AsEnumerable();

            return objects.FirstOrDefault();
        }

        public IQueryable<T> GetAll()
        {
            return Dbset.AsNoTracking();
        }

        public IQueryable<T> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAllDeleted()
        {
            throw new NotImplementedException();
        }

        public T GetAnyById(string id)
        {
            return Dbset.Find(id);
        }

        public T GetAnyById(long id)
        {
            return Dbset.Find(id);
        }

        public T GetAnyById(int id)
        {
            return Dbset.Find(id);
        }

        public T GetById(string id)
        {
            return Dbset.Find(id);
        }

        public T GetById(long id)
        {
            return Dbset.Find(id);
        }

        public T GetById(int id)
        {
            return Dbset.Find(id);
        }

     

     

        public IQueryable<T> IncludeSubSets(params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            Dbset.Attach(entity);
            DataContext.Entry(entity).State = EntityState.Modified;
        }

        #endregion



        #region IRepository<T> Members


        IEnumerable<T> IRepository<T>.GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetMany(System.Linq.Expressions.Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
