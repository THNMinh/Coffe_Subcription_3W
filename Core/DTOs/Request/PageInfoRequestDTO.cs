using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Constants;

namespace Core.DTOs.Request
{
    public class PageInfoRequestDTO
    {
        public int PageNum { get; set; } = Consts.PAGE_NUM_DEFAULT;
        public int PageSize { get; set; } = Consts.PAGE_SIZE_DEFAULT;
    }
}
