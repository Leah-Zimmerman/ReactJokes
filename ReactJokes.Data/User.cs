using System.Text.Json.Serialization;

namespace ReactJokes.Data
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
        public List<UserJoke> UserJokes { get; set; } = new List<UserJoke>();
    }
}