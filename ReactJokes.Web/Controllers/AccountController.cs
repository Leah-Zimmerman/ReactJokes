using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactJokes.Data;
using ReactJokes.Web.ViewModels;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace ReactJokes.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private string _connectionString;
        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [Route("signup")]
        [HttpPost]
        public void Signup(SignupViewModel svm)
        {
            var repo = new ReactJokesRepository(_connectionString);
            repo.Signup(svm, svm.Password);
        }
        [Route("login")]
        [HttpPost]
        public User Login(LoginViewModel credentials)
        {
            var repo = new ReactJokesRepository(_connectionString);
            var user = repo.Login(credentials.Email, credentials.Password);
            if (user == null)
            {
                return null;
            }
            var claims = new List<Claim>
            {
                new Claim("user", credentials.Email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                    new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            return user;

        }
        [Route("getcurrentuser")]
        public User GetCurrentUser()
        {
            if(!User.Identity.IsAuthenticated)
            {
                return null;
            }
            var repo = new ReactJokesRepository(_connectionString);
            return repo.GetByEmail(User.Identity.Name);
        }
        [Route("logout")]
        [HttpPost]
        public void Logout()
        {
            HttpContext.SignOutAsync().Wait();
        }
    }

}
