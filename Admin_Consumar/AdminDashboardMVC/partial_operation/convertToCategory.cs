using AdminDashboardMVC.DTO;
using AdminDashboardMVC.Models;

namespace admain.partial_operation
{
    static class convertToCategory
    {
        public static Category convert(CategoryViewModel categoryViewModel, string newimage)
        {
            Category category = new Category
            {
                Id= categoryViewModel.Id,
                Description= categoryViewModel.Description,
                Name= categoryViewModel.Name,
                Image = newimage,
                
            };
            return category;
        }
    }
}
