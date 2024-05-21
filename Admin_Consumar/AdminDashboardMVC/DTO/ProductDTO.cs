namespace AdminDashboardMVC.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public decimal? Weight { get; set; }
        public int? Stock { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public string CategoryName { get; set; }
        public int CategoryID { get; set; }
        public double AverageRating { get; set; }
    }
}
