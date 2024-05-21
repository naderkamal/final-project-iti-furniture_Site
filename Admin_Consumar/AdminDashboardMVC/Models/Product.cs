using System;
using System.Collections.Generic;

namespace AdminDashboardMVC.Models
{
    public partial class Product
    {
       

        public int Id { get; set; }
        public decimal? Weight { get; set; }
        public int? Stock { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
        public virtual ICollection<ProductUser> ProductUsers { get; set; }





        public Product()
        {
            
            ProductOrders = new HashSet<ProductOrder>();
            ProductUsers = new HashSet<ProductUser>();
        }
    }
}
