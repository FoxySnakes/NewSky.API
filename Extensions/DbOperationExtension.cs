using Microsoft.EntityFrameworkCore;
using NewSky.API.Models;
using NewSky.API.Models.Result;

namespace NewSky.API.Extensions
{
    public static class DbOperationExtension
    {
        public async static Task<PaginedResult<T>> PaginedAsync<T>(this IQueryable<T> list, PaginationParams pagination) where T : class
        {
            var result = new PaginedResult<T>() { };
            result.TotalCount = list.Count();
            result.PageNumber = pagination.PageNumber;
            result.PageSize = pagination.PageSize;
            result.Items = await list.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync();  

            return result;
        }
    }
}
