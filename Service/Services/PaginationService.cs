using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Request;
using Core.DTOs.Response;
using Core.Interfaces.Services;

namespace Service.Services
{
    public class PaginationService<T> : IPaginationService<T>
    {
        public PagingResponseDTO<T> GetPagedData(int totalItems, IEnumerable<T> data, PageInfoRequestDTO pageInfo)
        {
            int pageNum = pageInfo.PageNum;
            int pageSize = pageInfo.PageSize;
            int totalPages;
            if (pageSize == 0)
            {
                totalPages = 0;
            }
            else
            {
                totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            }

            return new PagingResponseDTO<T>
            {
                PageInfo = new PageInfoResponse
                {
                    PageNum = pageNum,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    TotalPages = totalPages
                },
                PageData = data
            };
        }
    }
}
