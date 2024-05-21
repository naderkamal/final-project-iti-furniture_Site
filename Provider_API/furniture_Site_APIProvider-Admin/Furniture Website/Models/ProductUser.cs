using System;
using System.Collections.Generic;

namespace Furniture_Website.Models
{
    public partial class ProductUser
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int? RatingStar { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual AspNetUser User { get; set; } = null!;
    }
}
