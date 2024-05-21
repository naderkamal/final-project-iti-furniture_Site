using System;
using System.Collections.Generic;

namespace AdminDashboardMVC.Models
{
    public partial class Review
    {
        

        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Comment { get; set; }
        public int UserID { get; set; }
        public virtual User Users { get; set; }
    }
}
