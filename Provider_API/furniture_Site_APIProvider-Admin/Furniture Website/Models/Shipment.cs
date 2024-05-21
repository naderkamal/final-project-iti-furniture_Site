using System;
using System.Collections.Generic;

namespace Furniture_Website.Models
{
    public partial class Shipment
    {
        public Shipment()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public bool? IsCompleted { get; set; }
        public decimal? Cost { get; set; }
        public DateTime? Date { get; set; }
        public string? Address { get; set; }
        public string? MoreDetails { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
