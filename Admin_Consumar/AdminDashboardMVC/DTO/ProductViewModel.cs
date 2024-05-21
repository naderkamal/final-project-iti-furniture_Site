using AdminDashboardMVC.Models;

namespace AdminDashboardMVC.DTO
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public decimal? Weight { get; set; }
        public int? Stock { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public string? Name { get; set; }
        public int CategoryID { get; set; }
    }
}
