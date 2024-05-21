using System.Collections.Generic;

namespace Furniture_Website.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Statue { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? ShipmentID { get; set; }
        public int? UserID { get; set; }
        public virtual List<int>? OrderProducts_IDs { get; set; }
        public OrderDTO()
        {
            OrderProducts_IDs = new List<int>(); 
        }

    }
}
