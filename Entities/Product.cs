using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;

namespace API.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }

        public DateTime Created { get; set; }

        public string PhotoUrl { get; set; }

        public List<ProductCategory> ProductCategories { get; set; } = new();

    }
}