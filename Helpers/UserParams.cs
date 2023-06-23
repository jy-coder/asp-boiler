namespace API.Helpers
{
    public class UserParams : PaginationParams
    {
        public string CurrentUsername { get; set; }
        public string OrderBy { get; set; } = "created";

    }


    public class ProductParams : PaginationParams
    {

        public string CategoryIds { get; set; }

    }
}



