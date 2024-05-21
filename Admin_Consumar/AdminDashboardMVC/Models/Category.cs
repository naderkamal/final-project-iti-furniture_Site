﻿using System;
using System.Collections.Generic;

namespace AdminDashboardMVC.Models
{
    public partial class Category
    {
        public int Id { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public Category() 
        {
            Products = new HashSet<Product>();
        }
    }
}
