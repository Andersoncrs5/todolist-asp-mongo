using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.API.utils.pagination
{
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalRecords { get; set; }
        public int Code { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
        public List<T> Data { get; set; }

        public PagedResponse(List<T> data, int pageNumber, int pageSize, long totalRecords, int Codes = 200)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            Code = Codes;
        }
    }
}