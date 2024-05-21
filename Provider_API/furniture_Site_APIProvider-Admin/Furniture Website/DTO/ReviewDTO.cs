namespace Furniture_Website.DTO
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public DateTime ? Date { get; set; }
        public string ?Comment { get; set; }

    }
}
