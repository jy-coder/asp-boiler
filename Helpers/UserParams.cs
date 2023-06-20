namespace API.Helpers
{
    public class UserParams : PaginationParams
    {
        public string CurrentUsername { get; set; }
        public string OrderBy { get; set; } = "created";
        public int? CategoryId { get; set; }
    }
}