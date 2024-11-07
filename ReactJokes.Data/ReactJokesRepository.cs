using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ReactJokes.Data
{
    public class ReactJokesRepository
    {
        private string _connectionString;
        public ReactJokesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Signup(User user, string password)
        {
            using var context = new ReactJokesDbContext(_connectionString);
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordHash = passwordHash;
            context.Users.Add(user);
            context.SaveChanges();
        }

        public User Login(string email,string password)
        {
            var user = GetByEmail(email);
            if(user==null)
            {
                return null;
            }
            var isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if(!isValidPassword)
            {
                return null;
            }
            return user;
        }
        public User GetByEmail(string email)
        {
            using var context = new ReactJokesDbContext(_connectionString);
            return context.Users.FirstOrDefault(u => u.Email == email);
        }
        public List<JokeWithLikes> GetJokesWithLikes()
        {
            var context = new ReactJokesDbContext(_connectionString);
            var jokes = context.Jokes
            .Include(j => j.UserJokes)
            .ToList();

            var jokesWithLikes = new List<JokeWithLikes>();
            foreach(var j in jokes)
            {
                jokesWithLikes.Add(new JokeWithLikes
                {
                    Joke = j,
                    LikesCount = j.UserJokes.Count(uj => uj.Liked),
                    DislikesCount = j.UserJokes.Count(uj => !uj.Liked)
                });
            }
            return jokesWithLikes;
        }

        public LikesForJoke GetLikesForJoke(int originId)
        {
            var context = new ReactJokesDbContext(_connectionString);
            var jokeWithUserJokes = context.Jokes
            .Include(j => j.UserJokes)
            .FirstOrDefault(j => j.OriginId == originId);

            var likes = jokeWithUserJokes.UserJokes.Count(uj => uj.Liked);
            var dislikes = jokeWithUserJokes.UserJokes.Count(uj => !uj.Liked);

            return new LikesForJoke
            {
                LikesCount = likes,
                DislikesCount = dislikes
            };
        }

        public void AddJoke(Joke joke)
        {
            using var context = new ReactJokesDbContext(_connectionString);
            if(context.Jokes.Any(j=>j.OriginId==joke.OriginId))
            {
                return;
            }
            context.Jokes.Add(joke);
            context.SaveChanges();
        }
        public void AddUserJoke (UserJoke userJoke)
        {
            using var context = new ReactJokesDbContext(_connectionString);
            context.UserJokes.Add(userJoke);
            context.SaveChanges();
        }
        public int GetJokeIdByOriginId(int originId)
        {
            using var context = new ReactJokesDbContext(_connectionString);
            var joke = context.Jokes.FirstOrDefault(j => j.OriginId == originId);
            return joke.Id;
        }

        public string GetUserInteraction(int userId, int jokeId)
        {
            using var context = new ReactJokesDbContext(_connectionString);
            var joke = context.UserJokes.FirstOrDefault(uj => uj.UserId == userId && uj.JokeId == jokeId);
            if(joke==null)
            {
                return null;
            }
            return joke.Liked ? "liked" : "disliked";
        }
        public void ChangeUserJokeLike(int userId, int jokeId)
        {
            using var context = new ReactJokesDbContext(_connectionString);
            var userJoke = context.UserJokes.FirstOrDefault(uj => uj.UserId == userId && uj.JokeId == jokeId);
            userJoke.Liked = !userJoke.Liked;
            userJoke.Time = DateTime.Now;
            context.SaveChanges();
        }
        public DateTime GetTime(int userId,int jokeId)
        {
            using var context = new ReactJokesDbContext(_connectionString);
            var userJoke = context.UserJokes.FirstOrDefault(uj => uj.UserId == userId && uj.JokeId == jokeId);
            if(userJoke==null)
            {
                throw new InvalidOperationException("User joke not found");
            }
            return userJoke.Time;

        }



    }

}