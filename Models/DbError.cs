using System.ComponentModel.DataAnnotations;

namespace NewSky.API.Models
{
    public class DbError
    {

        public DbError(DbErrorCode code, string entity)
        {
            Code = code;
            Entity = entity;
        }

        public DbError(DbErrorCode code)
        {
            Code = code;
        }

        [Required]
        public DbErrorCode Code { get; set; }

        [Required]
        public string Message => GetDbErrorMessage(Code);

        public string? Entity { get; set; } = null;

        private string GetDbErrorMessage(DbErrorCode code)
        {
           switch(code)
            {
                case DbErrorCode.DbFailedDuringSave:
                    return "Failed during saving data";
                case DbErrorCode.DbNoEntityWithId:
                    return "No Entity exist with this Id";
                case DbErrorCode.DbDuplicateKey:
                    return "Duplicated Key";
                case DbErrorCode.DbOther:
                    return "Unsuported DbError with Database";

                default:
                    return "Unsupported DbError Code";
            }
        }

    }

    public enum DbErrorCode
    {
        DbOther = 1,
        DbFailedDuringSave = 2,
        DbNoEntityWithId = 3,
        DbDuplicateKey = 4,       
    }
}
