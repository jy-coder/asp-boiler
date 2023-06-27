using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Product
{
    public class ProductUpdateDto
    {
        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public string PhotoUrl { get; set; }

        public List<PhotoDto> Photos { get; set; }

        public List<CategoryDto> Categories { get; set; }

        public List<int> CategoryIds { get; set; }

    }
}