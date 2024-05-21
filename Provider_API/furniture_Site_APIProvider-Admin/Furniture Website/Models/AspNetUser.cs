using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Furniture_Website.Models
{
    public partial class AspNetUser 
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserTokens = new HashSet<AspNetUserToken>();
            Orders = new HashSet<Order>();
            ProductUsers = new HashSet<ProductUser>();
            Reviews = new HashSet<Review>();
            Roles = new HashSet<AspNetRole>();
        }

        public int Id { get; set; }
        public int? Age { get; set; }
        public string? Ssn { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Governorate { get; set; }
        public string? Password { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? UserName { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ProductUser> ProductUsers { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<AspNetRole> Roles { get; set; }
    }
}
