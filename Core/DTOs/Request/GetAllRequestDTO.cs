using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Request
{
    public class GetAllRequestDTO
    {
        [Required(ErrorMessage = "Search condition is required")]
        public Search SearchCondition { get; set; } = null!;

        [Required(ErrorMessage = "Page information is required")]
        public PageInfoRequestDTO PageInfo { get; set; } = null!;
    }

    public class Search
    {
        public string? Keyword { get; set; }
        public bool IsDelete { get; set; }
    }
}
