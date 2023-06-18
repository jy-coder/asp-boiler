using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public string PhotoUrl { get; set; }

        public List<PhotoDto> Photos { get; set; }

        public List<CategoryDto> Categories { get; set; }

    }


    public class ProductUpdateDto
    {
        public string Description { get; set; }

        public decimal Price { get; set; }


        public DateTime Created { get; set; }

        public string PhotoUrl { get; set; }

        public List<PhotoDto> Photos { get; set; }

        public List<CategoryDto> Categories { get; set; }
    }


}