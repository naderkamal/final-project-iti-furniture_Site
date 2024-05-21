 using System.ComponentModel.DataAnnotations;

namespace AdminDashboardMVC.DTO
{
    public class ShipmentDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Is Completed is required")]
        public bool? IsCompleted { get; set; }
        [Required(ErrorMessage = "Cost is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid cost")]
        public decimal? Cost { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public DateTime? Date { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }
        public string? moreDetails { get; set; }
    }
}
