using System.Text.Json.Serialization;

namespace ReactJokes.Data
{
    public class Joke
    {
        public int Id { get; set; }
        public int OriginId { get; set; }
        public string Type { get; set; }
        public string Setup { get; set; }
        public string Punchline { get; set; }
        public List<UserJoke> UserJokes { get; set; } = new List<UserJoke>();
    }


}