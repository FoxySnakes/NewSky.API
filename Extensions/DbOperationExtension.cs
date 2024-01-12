using Microsoft.EntityFrameworkCore;
using NewSky.API.Models;
using NewSky.API.Models.Result;

namespace NewSky.API.Extensions
{
    public static class DbOperationExtension
    {
        public async static Task<PaginedResult<T>> PaginedAsync<T>(this IQueryable<T> list, int pageSize, int pageNumber) where T : class
        {
            var result = new PaginedResult<T>() { };
            result.TotalCount = list.Count();
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;
            result.Items = await list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();  

            return result;
        }
    }
}
