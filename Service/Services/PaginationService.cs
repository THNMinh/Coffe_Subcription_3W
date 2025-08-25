using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Constants;
using Core.DTOs.Request;
using Core.DTOs.Response;
using Core.Interfaces.Services;

namespace Service.Services
{
    public class PaginationService<T> : IPaginationService<T>
    {
        public PagingResponseDTO<T> GetPagedData(int totalItems, IEnumerable<T> data, PageInfoRequestDTO pageInfo)
        {
            int pageNum = pageInfo.PageNum == 0 ? Consts.PAGE_NUM_DEFAULT : pageInfo.PageNum;
            int pageSize = pageInfo.PageSize == 0 ? Consts.PAGE_SIZE_DEFAULT : pageInfo.PageSize;
            int totalPages = (pageSize == 0) ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);

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
