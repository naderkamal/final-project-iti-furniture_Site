using System;
using System.Collections.Generic;

namespace AdminDashboardMVC.Models
{
    public partial class Shipment
    {
        public int Id { get; set; }
        public bool? IsCompleted { get; set; }
        public decimal? Cost { get; set; }
        public DateTime? Date { get; set; }
        public string? Address { get; set; }
        public string? moreDetails { get; set; }

        // مش محتاج اضيفها ف ال (DTO) لانها دايما علاقه 1:1
        public virtual ICollection<Order> Orders { get; set; }
    }
}
