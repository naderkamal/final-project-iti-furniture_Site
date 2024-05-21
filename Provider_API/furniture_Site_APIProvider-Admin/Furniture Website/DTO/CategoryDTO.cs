﻿
namespace Furniture_Website.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }
        public List<ProductDTO>? Products { get; set; }
    }
}
