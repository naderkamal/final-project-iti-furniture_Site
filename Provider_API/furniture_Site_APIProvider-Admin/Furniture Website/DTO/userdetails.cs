namespace Furniture_Website.DTO
{
    public class userdetails
    {
        public int Id { get; set; }
        public int? Age { get; set; }
        public string? SSN { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Governorate { get; set; }
        public string? Role { get; set; }

        public List<int>? OrderIds { get; set; }
        public List<int>? ReviewIds { get; set; }
        public List<int>? ProductIds { get; set; }
    }
}
