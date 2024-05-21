using System;
using System.Collections.Generic;

namespace AdminDashboardMVC.Models
{
    public partial class Order
    {
        public Order()
        {
            ProductOrders = new HashSet<ProductOrder>();
            
        }

        public int Id { get; set; }
        public string? Statue { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? PhoneNumber { get; set; }
        public int? ShipmentID { get; set; }
        public int? UserID { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
        public virtual Shipment? Shipment { get; set; }
        public virtual User? User { get; set; }
    }
}
