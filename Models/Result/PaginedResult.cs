﻿namespace NewSky.API.Models.Result
{
    public class PaginedResult<T> where T : class
    {
        public List<T>? Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalCount / PageSize);
            }
        }

    }
}
