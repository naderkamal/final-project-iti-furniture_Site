using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AdminDashboardMVC.Models
{
    public partial class User : IdentityUser<int>
    {

        public int? Age { get; set; }
        public string? Ssn { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Governorate { get; set; }
        public string? Password { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<ProductUser> ProductUsers { get; set; }

        public User()
        {
            ProductUsers = new HashSet<ProductUser>();
            Orders = new HashSet<Order>();
            Reviews = new HashSet<Review>();
        }

    }
}
