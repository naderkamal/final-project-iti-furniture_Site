using System;
using System.Collections.Generic;

namespace AdminDashboardMVC.Models
{
    public partial class ProductUser
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int? RatingStar { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
