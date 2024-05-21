using AdminDashboardMVC.DTO;
using AdminDashboardMVC.Models;

namespace admain.partial_operation
{
    static class convertToProductviewmodel
    {
        public static ProductViewModel convert(Product product, IFormFile newimage)
        {
            ProductViewModel productviewModel = new ProductViewModel
            {
                Id = product.Id,
                Weight = product.Weight,
                Stock = product.Stock,
                Price = product.Price,
                Description = product.Description,
                Name = product.Name,
                CategoryID = product.CategoryID,
                Image = newimage
            };
            return productviewModel;
        }
    }
}
