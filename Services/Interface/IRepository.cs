using Microsoft.EntityFrameworkCore.ChangeTracking;
using NewSky.API.Models;
using NewSky.API.Models.Result;

namespace NewSky.API.Services.Interface
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<DbOperationResult<T>> CreateAsync(T entity);
        Task<DbOperationResult<T>> UpdateAsync(int entityId);
        Task<DbOperationResult<T>> DeleteAsync(int id);
        IQueryable<T> Where(Func<T, bool> predicate);
        public IQueryable<T> Query();
    }
}
