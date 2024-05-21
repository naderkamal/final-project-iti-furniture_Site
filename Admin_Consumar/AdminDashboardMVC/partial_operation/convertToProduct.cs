using AdminDashboardMVC.DTO;
using AdminDashboardMVC.Models;

namespace admain.partial_operation
{
    static class convertToProduct
    {  
        public static Product convert(ProductViewModel productviewModel,string newimage)
        {
            Product product = new Product
            {
                Id = productviewModel.Id,
                Weight = productviewModel.Weight,
                Stock = productviewModel.Stock,
                Price = productviewModel.Price,
                Description = productviewModel.Description,
                Name = productviewModel.Name,
                CategoryID = productviewModel.CategoryID,
                Image = newimage
            };
            return product;
        }
    }
}
