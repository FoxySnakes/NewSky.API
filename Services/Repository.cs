using Microsoft.Data.SqlClient;
using NewSky.API.Data;
using NewSky.API.Models;
using NewSky.API.Models.Result;
using NewSky.API.Services.Interface;

namespace NewSky.API.Services
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly NewSkyDbContext _dbContext;

        public Repository(NewSkyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DbOperationResult<T>> CreateAsync(T entity)
        {
            var result = new DbOperationResult<T>();
            try
            {
                var addedResult = await _dbContext.Set<T>().AddAsync(entity);
                var changeNumber = await _dbContext.SaveChangesAsync();
                result.Entity = addedResult.Entity;
                result.Errors = changeNumber > 0 ? new List<DbError>() : new List<DbError> { new DbError(DbErrorCode.DbFailedDuringSave) };
            }
            catch (Exception ex)
            {
                if(ex.InnerException is SqlException sqlException && sqlException.Number == 2601)
                {
                    var field = sqlException.Message.Split('»')[1].Split('_').Last().Trim();
                    result.Entity = entity;
                    result.Errors = new List<DbError>() { new DbError(DbErrorCode.DbDuplicateKey, field) };
                }
                else
                {
                    result.Entity = entity;
                    result.Errors = new List<DbError>() { new DbError(DbErrorCode.DbFailedDuringSave) };
                }
            }

            return result;
        }

        public async Task<DbOperationResult<T>> UpdateAsync(int id)
        {
            var dbOperationResult = new DbOperationResult<T>();
            var changeNumber = await _dbContext.SaveChangesAsync();
            dbOperationResult.Errors = changeNumber > 0 ? new List<DbError>() : new List<DbError> { new DbError(DbErrorCode.DbFailedDuringSave) };
            dbOperationResult.Entity = _dbContext.Set<T>().Find(id);
            return dbOperationResult;
        }

        public async Task<DbOperationResult<T>> DeleteAsync(int id)
        {
            var dbOperationResult = new DbOperationResult<T>();
            var entity = _dbContext.Set<T>().Find(id);
            if (entity == null)
            {
                dbOperationResult.Errors.Add(new DbError(DbErrorCode.DbNoEntityWithId, id.ToString()));
                dbOperationResult.Entity = null;
            }
            else
            {
                _dbContext.Set<T>().Remove(entity);
                var changeNumber = await _dbContext.SaveChangesAsync();
                dbOperationResult.Entity = entity;
                dbOperationResult.Errors = changeNumber > 0 ? new List<DbError>() : new List<DbError> { new DbError(DbErrorCode.DbFailedDuringSave) };
            }

            return dbOperationResult;
        }

        public IQueryable<T> Where(Func<T, bool> predicate)
        {
            var queryable = _dbContext.Set<T>().Where(predicate);
            return (IQueryable<T>)queryable;
        }

        public IQueryable<T> Query()
        {
            return _dbContext.Set<T>();
        }
    }
}
