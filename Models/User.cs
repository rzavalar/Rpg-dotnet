namespace dotnet__rpg.Models
{
    public class User
    {
        public int Id { get; set; }

        public String UserName { get; set; } = string.Empty;

        public byte[] PasswordHash {get;set;}

        public byte[] PasswordSalt{get;set;}

        public List<Character>? Characters { get; set; }
    }
}