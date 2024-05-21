using System;
using System.Collections.Generic;

namespace AdminDashboardMVC.Models
{
    public partial class ProductOrder
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal? ItemPrice { get; set; }
        public int? Count { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
