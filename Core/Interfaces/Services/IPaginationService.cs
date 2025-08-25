using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Request;
using Core.DTOs.Response;

namespace Core.Interfaces.Services
{
    public interface IPaginationService<T>
    {
        PagingResponseDTO<T> GetPagedData(int totalItems, IEnumerable<T> data, PageInfoRequestDTO pageInfo);
    }
}
