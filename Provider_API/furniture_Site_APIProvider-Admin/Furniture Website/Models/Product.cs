using System;
using System.Collections.Generic;

namespace Furniture_Website.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductOrders = new HashSet<ProductOrder>();
            ProductUsers = new HashSet<ProductUser>();
        }

        public int Id { get; set; }
        public decimal? Weight { get; set; }
        public int? Stock { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
        public virtual ICollection<ProductUser> ProductUsers { get; set; }
    }
}
