using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using Database;

namespace Database.Repository.BaseRepository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbContext context;
        private DbSet<T> entities;
        readonly string errorMessage = string.Empty;
        public BaseRepository(DbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public IEnumerable<T> RawQuery(string query)
        {

            return context.Database.SqlQuery<T>(query, null).ToList();
        }
         
        public int ExecuteSqlCommand(string query)
        {
            return context.Database.ExecuteSqlCommand(query, null);
        }
        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }

       
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return entities.Where(predicate);
        }


       

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return entities.Where(predicate);
        }

        public bool IsExist(Expression<Func<T, bool>> predicate)
        {
            return entities.Count(predicate) > 0 ? true : false;
        }
        public long Count(Expression<Func<T, bool>> predicate)
        {
            return entities.Count(predicate);
        }
        public IEnumerable<T> GetAll(DateTime dtmFrom, DateTime dtmTo, string ViewId)
        {
            return entities.AsEnumerable();
        }
        public T Get(string id)
        {
            return entities.Find(id);
        }
        
        public T Get(Expression<Func<T, bool>> predicate)
        {
            return entities.FirstOrDefault(predicate);
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await entities.FirstOrDefaultAsync(predicate);
        }
        public T Get(int id)
        {
            return entities.Find(id);
        }
        public T Get(long? id)
        {
            return entities.Find(id);
        }
        public T Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }


            entities.Add(entity);
            context.SaveChanges();
            return entity;
        }
        public IQueryable<T> Search(Expression<Func<T, bool>> predicate)
        {
            return entities.Where(predicate);
        }
        public T Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();

                return entity;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                var v1 = dbEx.Message;
                return null;
            }



        }
        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                entities.Remove(entity);
                context.SaveChanges();
            }
            catch (Exception ex)
            {

                var v1 = ex.Message;
            }

        }
        public T DeleteData(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                entities.Remove(entity);
                context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {

                var v1 = ex.Message;
                return null;
            }

        }
        public IEnumerable<T> DeleteRange(IEnumerable<T> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            var results = entities.RemoveRange(entity);
            context.SaveChanges();
            return results;
        }
        public bool Deletes(List<T> models)
        {
            try
            {
                if (models == null)
                {
                    throw new ArgumentNullException("entity");
                }
                entities.RemoveRange(models);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return false;
            }

        }
        public void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();

        }
        public IEnumerable<T> BulkInsert(IEnumerable<T> models)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new System.TimeSpan(0, 15, 0)))
            {
                DCEntities context = null;
                try
                {
                    context = new DCEntities();
                    context.Configuration.AutoDetectChangesEnabled = false;
                    int count = 0;
                    foreach (var entityToInsert in models)
                    { 
                        ++count;
                        context = AddToContext(context, entityToInsert, count, 100, true);
                    }
                    context.SaveChanges();
                }
                finally
                {
                    if (context != null)
                    {
                        context.Dispose();
                    }
                }
                scope.Complete();
                return models;
            }
        }
        private DCEntities AddToContext(DCEntities context, T entity, int count, int commitCount, bool recreateContext)
        {
            context.Set<T>().Add(entity);
            if (count % commitCount == 0)
            {
                try
                {
                    context.SaveChanges();

                    if (recreateContext)
                    {
                        context.Dispose();
                        context = new DCEntities();
                        context.Configuration.AutoDetectChangesEnabled = false;
                    }
                }
                catch (Exception ex)
                {

                    var v1 = ex.Message;
                }

            }
            return context;
        }
        private DCEntities UpdateToContext(DCEntities context, T entity, int count, int commitCount, bool recreateContext)
        {
            context.Set<T>().Attach(entity);
            if (count % commitCount == 0)
            {
                context.SaveChanges();
                if (recreateContext)
                {
                    context.Dispose();
                    context = new DCEntities();
                    context.Configuration.AutoDetectChangesEnabled = false;
                }
            }
            return context;

        }
        public IEnumerable<T> BulkUpdate(IEnumerable<T> models)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                DCEntities context = null;
                try
                {
                    context = new DCEntities();
                    //context.Configuration.AutoDetectChangesEnabled = false;
                    int count = 0;
                    foreach (var entityToUpdate in models)
                    {
                        ++count;
                        context = UpdateToContext(context, entityToUpdate, count, 100, true);
                    }
                    context.SaveChanges();
                }
                finally
                {
                    if (context != null)
                        context.Dispose();
                }
                scope.Complete();
                return models;
            }
        }
        public async Task<T> InsertAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                entity = entities.Add(entity);
                await context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                context.Entry(entity).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return entity;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                var v1 = dbEx.Message;
                return null;
            }

        }
        public async Task<int> DeleteAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                entities.Remove(entity);
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                var v1 = ex.Message;
                return -1;
            }
        }
    }
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> RawQuery(string query);
        T Get(string id);
        T Insert(T entity);
        T Update(T entity);
        Task<T> InsertAsync(T entity);
        Task<T> UpdateAsync(T entity);
        void Delete(T entity);
        IQueryable<T> Search(Expression<Func<T, bool>> predicate);
        IEnumerable<T> BulkInsert(IEnumerable<T> entities);         
        IEnumerable<T> BulkUpdate(IEnumerable<T> entities);
        IEnumerable<T> DeleteRange(IEnumerable<T> entity);
    }
  
}
