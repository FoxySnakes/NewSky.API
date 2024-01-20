using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace NewSky.API.Models.Result
{
    public class DbOperationResult<T>
    {
        public DbOperationResult()
        {
            Table = typeof(T).Name.ToString();
        }
        public string Table { get; protected set; }

        [Required]
        public T Entity { get; set; }

        public bool IsSuccess
        {
            get
            {
                return Errors.IsNullOrEmpty();
            }
        }

        public List<DbError> Errors { get; set; } = new List<DbError>();
    }
}
