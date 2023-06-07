namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }


        public byte[] PasswordSalt { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public List<Photo> Photos { get; set; } = new();


    }
}