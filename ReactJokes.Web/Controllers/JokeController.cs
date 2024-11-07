using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactJokes.Data;
using ReactJokes.Web.ViewModels;
using System.Text.Json;

namespace ReactJokes.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokeController : ControllerBase
    {
        private string _connectionString;
        public JokeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [Route("getjoke")]
        [HttpGet]
        public Joke GetJoke()
        {
            var client = new HttpClient();
            var response = client.GetStringAsync("https://jokesapi.lit-projects.com/jokes/programming/random").Result;
            var jsonList = JsonSerializer.Deserialize<List<APIJoke>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var firstJoke = jsonList[0];
            var joke = new Joke
            {
                OriginId = firstJoke.JokeId,
                Type = firstJoke.Type,
                Setup = firstJoke.Setup,
                Punchline = firstJoke.Punchline
            };
            var repo = new ReactJokesRepository(_connectionString);
            repo.AddJoke(joke);
            return joke;
        }

        [Route("getLikesForJoke")]
        [HttpGet]
        public LikesForJoke GetLikesForJoke(int id)
        {
            var repo = new ReactJokesRepository(_connectionString);
            return repo.GetLikesForJoke(id);
        }

        [Route("addUserJoke")]
        [HttpPost]
        public void AddUserJoke(LikedViewModel lvm)
        {
            var email = User.Identity.Name;
            var repo = new ReactJokesRepository(_connectionString);
            var user = repo.GetByEmail(email);
            var jokeId = repo.GetJokeIdByOriginId(lvm.JokeId);
            //try
            //{
            var interaction = repo.GetUserInteraction(user.Id, jokeId);
            if (interaction == null)
            {
                try
                {
                    repo.AddUserJoke(new UserJoke
                    {
                        UserId = user.Id,
                        JokeId = jokeId,
                        Liked = lvm.Liked,
                        Time = DateTime.Now
                    });
                }
                catch(Exception e)
                {
                    throw new InvalidOperationException("Error adding user joke", e);
                }
            }
            else
            {
                repo.ChangeUserJokeLike(user.Id, jokeId);
            }

        }
        [Route("getUserInteraction")]
        [HttpGet]
        [Authorize]
        public string GetUserInteraction(int originId)
        {
            var email = User.Identity.Name;
            var repo = new ReactJokesRepository(_connectionString);
            var user = repo.GetByEmail(email);
            var jokeId = repo.GetJokeIdByOriginId(originId);
            var userInteraction = repo.GetUserInteraction(user.Id, jokeId);
            return userInteraction;
        }
        [Route("getJokesWithLikes")]
        [HttpGet]
        public List<JokeWithLikes> GetJokesWithLikes()
        {
            var repo = new ReactJokesRepository(_connectionString);
            return repo.GetJokesWithLikes();
        }
        [Route("getTime")]
        [HttpGet]
        public DateTime GetTime(int originId)
        {
            var repo = new ReactJokesRepository(_connectionString);
            var jokeId = repo.GetJokeIdByOriginId(originId);
            var email = User.Identity.Name;
            var user = repo.GetByEmail(email);
            return  repo.GetTime(user.Id, jokeId);
        }
    }
}
