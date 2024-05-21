using AdminDashboardMVC.DTO;
using AdminDashboardMVC.Models;

namespace admain.partial_operation
{
    static class convertToCategoryviewmodel
    {
        public static CategoryViewModel convert(Category category, IFormFile newimage)
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel
            {
                Id = category.Id,
                Description = category.Description,
                Name = category.Name,
                Image = newimage,

            };
            return categoryViewModel;
        }
    }
}
