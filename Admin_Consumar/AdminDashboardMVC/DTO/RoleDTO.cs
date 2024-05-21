namespace AdminDashboardMVC.DTO
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public virtual List<int>? Users_IDs { get; set; }
        public RoleDTO()
        {
            Users_IDs = new List<int>();
        }
    }
}
