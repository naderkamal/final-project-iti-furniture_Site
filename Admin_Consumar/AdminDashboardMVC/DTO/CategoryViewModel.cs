using AdminDashboardMVC.Models;

namespace AdminDashboardMVC.DTO
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public IFormFile? Image { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }
    }
}
