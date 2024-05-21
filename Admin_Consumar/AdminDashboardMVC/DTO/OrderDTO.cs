using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdminDashboardMVC.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Phone Number field is required.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "The Phone Number must be exactly 11 characters.")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "The Statue field is required.")]
        public string? Statue { get; set; }
        [Required(ErrorMessage = "The Total Price field is required.")]
        public decimal? TotalPrice { get; set; }
        [Required(ErrorMessage = "The Order Date field is required.")]
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
