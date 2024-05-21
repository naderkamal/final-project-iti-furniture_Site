using System;
using System.Collections.Generic;

namespace Furniture_Website.Models
{
    public partial class Review
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Comment { get; set; }
        public int UserId { get; set; }

        public virtual AspNetUser User { get; set; } = null!;
    }
}
