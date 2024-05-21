using System.ComponentModel.DataAnnotations;

namespace AdminDashboardMVC.DTO
{
    public class ReviewDetailsDTO
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string? Email { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy }")]
        public DateTime? Date { get; set; }
        public string? Comment { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
      
    }
}
