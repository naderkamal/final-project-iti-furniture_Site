using System.ComponentModel.DataAnnotations;

namespace AdminDashboardMVC.DTO
{
    public class Users
    {
        public int Id { get; set; }
        [Range(18, 120, ErrorMessage = "Age must be between 18 and 120.")]
        public int? Age { get; set; }

        [RegularExpression(@"^\d{14}$", ErrorMessage = "SSN must be a 14-digit number.")]
        public string? SSN { get; set; }

        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be a 11-digit number.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string? FirstName { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? Governorate { get; set; }

        public string? Role { get; set; }

        public int? RoleId { get; set; }

        public List<int>? OrderIds { get; set; }

        public List<int>? ReviewIds { get; set; }

        public List<int>? ProductIds { get; set; }
    }
}
