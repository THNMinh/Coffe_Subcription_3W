using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Response
{
    public class PagingResponseDTO<T>
    {
        public PageInfoResponse PageInfo { get; set; } = null!;
        public IEnumerable<T> PageData { get; set; } = null!;
    }
    public class PageInfoResponse
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int PageNum { get; set; }
        public int PageSize { get; set; }
    }
}
