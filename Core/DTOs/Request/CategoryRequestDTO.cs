using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Request
{
    public class CategoryRequestDTO
    {
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
    }
}
