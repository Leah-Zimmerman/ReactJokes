using System.Text.Json.Serialization;

namespace ReactJokes.Data
{
    public class UserJoke
    {
        public int UserId { get; set; }
        public int JokeId { get; set; }
        public bool Liked { get; set; }
        public DateTime Time { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public Joke Joke { get; set; }
    }
}